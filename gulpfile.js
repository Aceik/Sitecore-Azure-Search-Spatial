var gulp = require("gulp");
var msbuild = require("gulp-msbuild");
var debug = require("gulp-debug");
var foreach = require("gulp-foreach");
var rename = require("gulp-rename");
var newer = require("gulp-newer");
var util = require("gulp-util");
var runSequence = require("run-sequence");
var nugetRestore = require('gulp-nuget-restore');
var fs = require('fs');
var yargs = require("yargs").argv;
var unicorn = require("./scripts/unicorn.js");
var habitat = require("./scripts/habitat.js");
var helix = require("./scripts/helix.js");
var del = require('del');

var config;
if (fs.existsSync('./gulp-config.js.user')) {
    config = require("./gulp-config.js.user")();
}
else {
    config = require("./gulp-config.js")()
}

module.exports.config = config;

helix.header("The Habitat source code, tools and processes are examples of Sitecore Helix.", "Habitat is not supported by Sitecore and should be used at your own risk.");

gulp.task("default", function (callback) {
  config.runCleanBuilds = true;
  return runSequence(
    "01-Copy-Sitecore-License",
    "02-Nuget-Restore",
    "03-Publish-All-Projects",
    "04-Apply-Xml-Transform",
	"Publish-All-Configs",
    "05-Sync-Unicorn",
    "06-Deploy-Transforms",
	callback);
});

gulp.task("deploy", function (callback) {
  config.runCleanBuilds = true;
  return runSequence(
    "01-Copy-Sitecore-License",
    "02-Nuget-Restore",
    "03-Publish-All-Projects",
    "04-Apply-Xml-Transform",
	"06-Deploy-Transforms",
	callback);
});

/*****************************
  Initial setup
*****************************/
gulp.task("01-Copy-Sitecore-License", function () {
  console.log("Copying Sitecore License file");

  return gulp.src(config.licensePath).pipe(gulp.dest("./lib"));
});

gulp.task("02-Nuget-Restore", function (callback) {
  var solution = "./" + config.solutionName + ".sln";
  return gulp.src(solution).pipe(nugetRestore());
});


gulp.task("03-Publish-All-Projects", function (callback) {
  return runSequence(
    "Build-Solution",
    "Publish-Foundation-Projects",
    "Publish-Feature-Projects",
    "Publish-Project-Projects", callback);
});

gulp.task("04-Apply-Xml-Transform", function () {
  var layerPathFilters = ["./src/Foundation/**/*.transform", "./src/Feature/**/*.transform", "./src/Project/**/*.transform", "!./src/**/obj/**/*.transform", "!./src/**/bin/**/*.transform"];

   var gulpRanInThisFolder = process.cwd();

  return gulp.src(layerPathFilters)
    .pipe(foreach(function (stream, file) {
        var fileToTransform = file.path.replace(/.+code\\*\\(.+)\.transform/, "$1");

        // Found a bug with C drive and ConnectionStrings.config.transform a little bit frustrating
        if (fileToTransform.indexOf("C:") > -1 && fileToTransform.indexOf("Spatial") > -1) {
            fileToTransform = fileToTransform.replace(gulpRanInThisFolder, "");
            fileToTransform = fileToTransform.replace("\\src\\Foundation\\Spatial\\Code\\", "");
            fileToTransform = fileToTransform.replace(".transform", "");
            util.log("ConnectionStrings.config.transform Found: " + fileToTransform);
        }
      util.log("fileToTransform: " + fileToTransform);
      util.log("Applying configuration transform: " + file.path);
      return gulp.src("./scripts/applytransform.targets")
        .pipe(msbuild({
          targets: ["ApplyTransform"],
          configuration: config.buildConfiguration,
          logCommand: false,
          verbosity: config.buildVerbosity,
          stdout: true,
          errorOnFail: true,
          maxcpucount: config.buildMaxCpuCount,
          nodeReuse: false,
          toolsVersion: config.buildToolsVersion,
          properties: {
            Platform: config.buildPlatform,
            WebConfigToTransform: config.websiteRoot,
            TransformFile: file.path,
            FileToTransform: fileToTransform
          }
        }));
    }));
});

gulp.task("05-Sync-Unicorn", function (callback) {
  var options = {};
  options.siteHostName = habitat.getSiteUrl();
  options.authenticationConfigFile = config.websiteRoot + "/App_config/Include/Unicorn.SharedSecret.config";

  unicorn(function() { return callback() }, options);
});


gulp.task("06-Deploy-Transforms", function () {
  return gulp.src("./src/**/code/**/*.transform")
      .pipe(gulp.dest(config.websiteRoot + "/temp/transforms"));
});

/*****************************
  Copy assemblies to all local projects
*****************************/
gulp.task("Copy-Local-Assemblies", function () {
  console.log("Copying site assemblies to all local projects");
  var files = config.sitecoreLibraries + "/**/*";

  var root = "./src";
  var projects = root + "/**/code/bin";
  return gulp.src(projects, { base: root })
    .pipe(foreach(function (stream, file) {
      console.log("copying to " + file.path);
      gulp.src(files)
        .pipe(gulp.dest(file.path));
      return stream;
    }));
});

/*****************************
  Publish
*****************************/
var publishStream = function (stream, dest) {
  var targets = ["Build"];

  return stream
    .pipe(debug({ title: "Building project:" }))
    .pipe(msbuild({
      targets: targets,
      configuration: config.buildConfiguration,
      logCommand: false,
      verbosity: config.buildVerbosity,
      stdout: true,
      errorOnFail: true,
      maxcpucount: config.buildMaxCpuCount,
      nodeReuse: false,
      toolsVersion: config.buildToolsVersion,
      properties: {
        Platform: config.publishPlatform,
        DeployOnBuild: "true",
        DeployDefaultTarget: "WebPublish",
        WebPublishMethod: "FileSystem",
        DeleteExistingFiles: "false",
        publishUrl: dest,
        _FindDependencies: "false"
      }
    }));
}

var publishProject = function (location, dest) {
  dest = dest || config.websiteRoot;

  console.log("publish to " + dest + " folder");
  return gulp.src(["./src/" + location + "/code/*.csproj"])
    .pipe(foreach(function (stream, file) {
      return publishStream(stream, dest);
    }));
}

var publishProjects = function (location, dest) {
  dest = dest || config.websiteRoot;

  console.log("publish to " + dest + " folder");
  return gulp.src([location + "/**/code/*.csproj"])
    .pipe(foreach(function (stream, file) {
      return publishStream(stream, dest);
    }));
};

gulp.task("Build-Solution", function () {
  var targets = ["Build"];
  if (config.runCleanBuilds) {
    targets = ["Clean", "Build"];
  }

  var solution = "./" + config.solutionName + ".sln";
  return gulp.src(solution)
      .pipe(msbuild({
          targets: targets,
          configuration: config.buildConfiguration,
          logCommand: false,
          verbosity: config.buildVerbosity,
          stdout: true,
          errorOnFail: true,
          maxcpucount: config.buildMaxCpuCount,
          nodeReuse: false,
          toolsVersion: config.buildToolsVersion,
          properties: {
            Platform: config.buildPlatform
          }
        }));
});

gulp.task("Publish-Foundation-Projects", function () {
  return publishProjects("./src/Foundation");
});

gulp.task("Publish-Feature-Projects", function () {
  return publishProjects("./src/Feature");
});

gulp.task("Publish-Project-Projects", function () {
  return publishProjects("./src/Project");
});

gulp.task("Publish-Project", function () {
  if(yargs && yargs.m && typeof(yargs.m) == 'string') {
    return publishProject(yargs.m);
  } else {
    throw "\n\n------\n USAGE: -m Layer/Module \n------\n\n";
  }
});

gulp.task("Publish-Assemblies", function () {
  var root = "./src";
  var binFiles = root + "/**/code/**/bin/Sitecore.{Feature,Foundation,Habitat}.*.{dll,pdb}";
  var destination = config.websiteRoot + "/bin/";
  return gulp.src(binFiles, { base: root })
    .pipe(rename({ dirname: "" }))
    .pipe(newer(destination))
    .pipe(debug({ title: "Copying " }))
    .pipe(gulp.dest(destination));
});

gulp.task("Publish-All-Views", function () {
  var root = "./src";
  var roots = [root + "/**/Views", "!" + root + "/**/obj/**/Views"];
  var files = "/**/*.cshtml";
  var destination = config.websiteRoot + "\\Views";
  return gulp.src(roots, { base: root }).pipe(
    foreach(function (stream, file) {
      console.log("Publishing from " + file.path);
      gulp.src(file.path + files, { base: file.path })
        .pipe(newer(destination))
        .pipe(debug({ title: "Copying " }))
        .pipe(gulp.dest(destination));
      return stream;
    })
  );
});

gulp.task("Publish-All-Configs", function () {
  var root = "./src";
  var roots = [root + "/**/App_Config", "!" + root + "/**/obj/**/App_Config"];
  var files = "/**/*.config";
  var destination = config.websiteRoot + "\\App_Config";
  return gulp.src(roots, { base: root }).pipe(
    foreach(function (stream, file) {
      console.log("Publishing from " + file.path);
      gulp.src(file.path + files, { base: file.path })
        .pipe(newer(destination))
        .pipe(debug({ title: "Copying " }))
        .pipe(gulp.dest(destination));
      return stream;
    })
  );
});

/*****************************
 Watchers
*****************************/
gulp.task("Auto-Publish-Css", function () {
  var root = "./src";
  var roots = [root + "/**/styles", "!" + root + "/**/obj/**/styles"];
  var files = "/**/*.css";
  var destination = config.websiteRoot + "\\styles";
  gulp.src(roots, { base: root }).pipe(
    foreach(function (stream, rootFolder) {
      gulp.watch(rootFolder.path + files, function (event) {
        if (event.type === "changed") {
          console.log("publish this file " + event.path);
          gulp.src(event.path, { base: rootFolder.path }).pipe(gulp.dest(destination));
        }
        console.log("published " + event.path);
      });
      return stream;
    })
  );
});

gulp.task("Auto-Publish-Views", function () {
  var root = "./src";
  var roots = [root + "/**/Views", "!" + root + "/**/obj/**/Views"];
  var files = "/**/*.cshtml";
  var destination = config.websiteRoot + "\\Views";
  return gulp.src(roots, { base: root }).pipe(
    foreach(function (stream, rootFolder) {
      gulp.watch(rootFolder.path + files, function (event) {
        if (event.type === "changed") {
          console.log("publish this file " + event.path);
          gulp.src(event.path, { base: rootFolder.path }).pipe(gulp.dest(destination));
        }
        console.log("published " + event.path);
      });
      return stream;
    })
  );
});

gulp.task("Auto-Publish-Assemblies", function () {
  var root = "./src";
  var roots = [root + "/**/code/**/bin"];
  var files = "/**/Sitecore.{Feature,Foundation,Habitat}.*.{dll,pdb}";;
  var destination = config.websiteRoot + "/bin/";
  gulp.src(roots, { base: root }).pipe(
    foreach(function (stream, rootFolder) {
      gulp.watch(rootFolder.path + files, function (event) {
        if (event.type === "changed") {
          console.log("publish this file " + event.path);
          gulp.src(event.path, { base: rootFolder.path }).pipe(gulp.dest(destination));
        }
        console.log("published " + event.path);
      });
      return stream;
    })
  );
});

/*****************************
 Package
*****************************/
var websiteRootBackup = config.websiteRoot;
var path = require("path");
var rimrafDir = require("rimraf");
var rimraf = require("gulp-rimraf");
var xmlpoke = require("xmlpoke");

/* publish files to temp location */
gulp.task("Package-Publish", function (callback) {
    config.websiteRoot = path.resolve("./temp");
    config.buildConfiguration = "Release";
    fs.mkdirSync(config.websiteRoot);
    runSequence(
      "Build-Solution",
      "Publish-Foundation-Projects",
      "Publish-Feature-Projects",
      "Publish-Project-Projects", callback);
});

/* Remove unwanted files */
gulp.task("Package-Prepare-Package-Files", function (callback) {
    var excludeList = [
      config.websiteRoot + "\\bin\\{Sitecore,Lucene,Newtonsoft,System,Microsoft.Web.Infrastructure}*dll",
      config.websiteRoot + "\\compilerconfig.json.defaults",
      config.websiteRoot + "\\packages.config",
      config.websiteRoot + "\\App_Config\\Include\\{Feature,Foundation,Project}\\*Serialization.config",
      config.websiteRoot + "\\App_Config\\Include\\{Feature,Foundation,Project}\\z.*DevSettings.config",
      "!" + config.websiteRoot + "\\bin\\Sitecore.Support*dll",
      "!" + config.websiteRoot + "\\bin\\Sitecore.{Feature,Foundation,Habitat,Demo,Common}*dll"
    ];
    console.log(excludeList);

    return gulp.src(excludeList, { read: false }).pipe(rimraf({ force: true }));
});

/* Add files to package definition */
gulp.task("Package-Enumerate-Files", function () {
    var packageFiles = [];
    config.websiteRoot = websiteRootBackup;

    return gulp.src(path.resolve("./temp") + "/**/*.*", { base: "temp", read: false })
      .pipe(foreach(function (stream, file) {
          var item = "/" + file.relative.replace(/\\/g, "/");
          console.log("Added to the package:" + item);
          packageFiles.push(item);
          return stream;
      })).pipe(util.buffer(function () {
          xmlpoke("./package.xml", function (xml) {
              for (var idx in packageFiles) {
                  xml.add("project/Sources/xfiles/Entries/x-item", packageFiles[idx]);
              }
          });
      }));
});

/* Add items to package definition */
gulp.task("Package-Enumerate-Items", function () {
    var itemPaths = [];
    var allowedPatterns = [
      "./src/**/serialization/**/*.yml",
      "!./src/**/serialization/Roles/**/*.yml",
      "!./src/**/serialization/Users/**/*.yml"
    ];
    return gulp.src(allowedPatterns)
        .pipe(foreach(function (stream, file) {
        console.log(file);
            var itemPath = unicorn.getFullItemPath(file);
            itemPaths.push(itemPath);
            return stream;
        })).pipe(util.buffer(function () {
            xmlpoke("./package.xml", function (xml) {
                for (var idx in itemPaths) {
                    xml.add("project/Sources/xitems/Entries/x-item", itemPaths[idx]);
                }
            });
        }));
});

/* Add users to package definition */
gulp.task("Package-Enumerate-Users", function () {
    var users = [];

    return gulp.src("./src/**/serialization/Users/**/*.yml")
        .pipe(foreach(function (stream, file) {
          console.log(file);
            var fileContent = file.contents.toString();
            var userName = unicorn.getUserPath(file);
            users.push(userName);
            return stream;
        })).pipe(util.buffer(function () {
            xmlpoke("./package.xml", function (xml) {
                for (var idx in users) {
                    xml.add("project/Sources/accounts/Entries/x-item", users[idx]);
                }
            });
        }));
});

/* Add roles to package definition */
gulp.task("Package-Enumerate-Roles", function () {
    var roles = [];

    return gulp.src("./src/**/serialization/Roles/**/*.yml")
        .pipe(foreach(function (stream, file) {
          console.log(file);
            var fileContent = file.contents.toString();
            var roleName = unicorn.getRolePath(file);            
            roles.push(roleName);
            return stream;
        })).pipe(util.buffer(function () {
            xmlpoke("./package.xml", function (xml) {
                for (var idx in roles) {
                    xml.add("project/Sources/accounts/Entries/x-item", roles[idx]);
                }
            });
        }));
});

/* Remove temp files */
gulp.task("Package-Clean", function (callback) {
    rimrafDir.sync(path.resolve("./temp"));
    callback();
});

/* Main task, generate package.xml */
gulp.task("Package-Generate", function (callback) {
    runSequence(
        "Package-Clean",
        "Package-Publish",
        "Package-Prepare-Package-Files",
        "Package-Enumerate-Files",
        "Package-Enumerate-Items",
        "Package-Enumerate-Users",
        "Package-Enumerate-Roles",
        "Package-Clean",
        callback);
});


/*****************************
  https://doc.sitecore.net/sitecore_experience_platform/setting_up_and_maintaining/search_and_indexing/configure_azure_search#_Create_a_Search
  // If running on Azure Search we need to disable lucene default configs
*****************************/
gulp.task('Z-Search-Disable-Lucene-Configs', function () {

    var root = config.websiteRoot + "/App_Config/Include/";
    var socialRoot = config.websiteRoot + "/App_Config/Include/social/";
    var listManagementRoot = config.websiteRoot + "/App_Config/Include/ListManagement/";
    var fxmRoot = config.websiteRoot + "/App_Config/Include/FXM/";
    var testingRoot = config.websiteRoot + "/App_Config/Include/ContentTesting/";

    // Main root config folder
    var roots = [root + "Sitecore.ContentSearch.Lucene.DefaultIndexConfiguration.config",
        root + "Sitecore.ContentSearch.Lucene.Index.Analytics.config",
        root + "Sitecore.ContentSearch.Lucene.Index.Core.config",
        root + "Sitecore.ContentSearch.Lucene.Index.Master.config",
        root + "Sitecore.ContentSearch.Lucene.Index.Web.config",

        root + "Sitecore.Marketing.Lucene.Index.Master.config",
        root + "Sitecore.Marketing.Lucene.Index.Web.config",
        root + "Sitecore.Marketing.Lucene.IndexConfiguration.config",
        root + "Sitecore.Marketing.Definitions.MarketingAssets.Repositories.Lucene.Index.Master.config",
        root + "Sitecore.Marketing.Definitions.MarketingAssets.Repositories.Lucene.Index.Web.config",
        root + "Sitecore.Marketing.Definitions.MarketingAssets.Repositories.Lucene.IndexConfiguration.config",

        root + "Sitecore.Speak.ContentSearch.Lucene.config",
        root + "Sitecore.Marketing.Definitions.MarketingAssets.Repositories.Lucene.Index.Web.config",
        root + "Sitecore.Marketing.Definitions.MarketingAssets.Repositories. Lucene.IndexConfiguration.config",

        root + "Sitecore.Speak.ContentSearch.Lucene.config",
        root + "Sitecore.ContentSearch.Lucene.DefaultIndexConfiguration.Xdb.config"

    ];


    var fxmFiles = [
        fxmRoot + "Sitecore.FXM.Lucene.DomainsSearch.DefaultIndexConfiguration.config",
        fxmRoot + "Sitecore.FXM.Lucene.DomainsSearch.Index.Master.config",
        fxmRoot + "Sitecore.FXM.Lucene.DomainsSearch.Index.Web.config",
    ];


    var listManFiles = [
        listManagementRoot + "Sitecore.ListManagement.Lucene.Index.List.config",
        listManagementRoot + "Sitecore.ListManagement.Lucene.IndexConfiguration.config",
    ];


    var socialsFiles = [
        socialRoot + "Sitecore.Social.Lucene.Index.Master.config",
        socialRoot + "Sitecore.Social.Lucene.Index.Web.config",
        socialRoot + "Sitecore.Social.Lucene.IndexConfiguration.config",
        socialRoot + "Sitecore.Social.Lucene.Index.Analytics.Facebook.config"
    ];


    var testingFiles = [
        testingRoot + "Sitecore.ContentTesting.Lucene.IndexConfiguration.config",
    ];

    return Promise.all([
        new Promise(function (resolve, reject) {
            
            gulp.src(roots).pipe(rename(function (path) {
                path.extname = ".config.disabled";
            })).pipe(gulp.dest(root));


            gulp.src(fxmFiles).pipe(rename(function (path) {
                path.extname = ".config.disabled";
            })).pipe(gulp.dest(fxmRoot));

            gulp.src(listManFiles)
                .pipe(rename(function (path) {
                    path.extname = ".config.disabled";
                })).pipe(gulp.dest(listManagementRoot));

            gulp.src(socialsFiles).pipe(rename(function (path) {
                path.extname = ".config.disabled";
            })).pipe(gulp.dest(socialRoot));

            gulp.src(testingFiles).pipe(rename(function (path) { path.extname = ".config.disabled"; }))
                .pipe(gulp.dest(testingRoot));

        }),
        new Promise(function (resolve, reject) {
            del(roots, { force: true });
            del(fxmFiles, { force: true });
            del(listManFiles, { force: true });
            del(socialsFiles, { force: true });
            del(testingFiles, { force: true });
        })
    ]).then(function () {
        // Other actions
        del(roots, { force: true });
        del(fxmFiles, { force: true });
        del(listManFiles, { force: true });
        del(socialsFiles, { force: true });
        del(testingFiles, { force: true });
    });

});

gulp.task('Z-Search-Enable-Azure-Configs', function () {

    var root = config.websiteRoot + "/App_Config/Include/";
    var socialRoot = config.websiteRoot + "/App_Config/Include/social/";
    var listManagementRoot = config.websiteRoot + "/App_Config/Include/ListManagement/";
    var fxmRoot = config.websiteRoot + "/App_Config/Include/FXM/";
    var testingRoot = config.websiteRoot + "/App_Config/Include/ContentTesting/";

    // Main root config folder
    var roots = [root + "Sitecore.ContentSearch.Azure.DefaultIndexConfiguration.config.disabled",
        root + "Sitecore.ContentSearch.Azure.Index.Analytics.config.disabled",
        root + "Sitecore.ContentSearch.Azure.Index.Core.config.disabled",
        root + "Sitecore.ContentSearch.Azure.Index.Master.config.disabled",
        root + "Sitecore.ContentSearch.Azure.Index.Web.config.disabled",

        root + "Sitecore.Marketing.Azure.Index.Master.config.disabled",
        root + "Sitecore.Marketing.Azure.Index.Web.config.disabled",
        root + "Sitecore.Marketing.Azure.IndexConfiguration.config.disabled",
        root + "Sitecore.Marketing.Definitions.MarketingAssets.Repositories.Azure.Index.Master.config.disabled",
        root + "Sitecore.Marketing.Definitions.MarketingAssets.Repositories.Azure.Index.Web.config.disabled",
        root + "Sitecore.Marketing.Definitions.MarketingAssets.Repositories.Azure.IndexConfiguration.config.disabled"
    ];

    gulp.src(roots).pipe(rename(function (path) {
        path.extname = ""; path.basename = path.basename.replace(".disabled");
    })).pipe(gulp.dest(root));

    //del(roots, { force: true });

    // Testing files

    var testingFiles = [
        testingRoot + "Sitecore.ContentTesting.Azure.IndexConfiguration.config.disabled",
    ];

    gulp.src(testingFiles).pipe(rename(function (path) { path.extname = ""; path.basename = path.basename.replace(".disabled"); }))
        .pipe(gulp.dest(testingRoot));
    //del(testingFiles, { force: true });

    // FXM files

    var fxmFiles = [
        fxmRoot + "Sitecore.FXM.Azure.DomainsSearch.DefaultIndexConfiguration.config.disabled",
        fxmRoot + "Sitecore.FXM.Azure.DomainsSearch.Index.Master.config.disabled",
        fxmRoot + "Sitecore.FXM.Azure.DomainsSearch.Index.Web.config.disabled",
    ];

    gulp.src(fxmFiles).pipe(rename(function (path) {
        path.extname = ""; path.basename = path.basename.replace(".disabled");
    })).pipe(gulp.dest(fxmRoot));

    //del(fxmFiles, { force: true });

    var listManFiles = [
        listManagementRoot + "Sitecore.ListManagement.Azure.Index.List.config.disabled",
        listManagementRoot + "Sitecore.ListManagement.Azure.IndexConfiguration.config.disabled",
    ];

    gulp.src(listManFiles)
        .pipe(rename(function (path) {
            path.extname = ""; path.basename = path.basename.replace(".disabled");
        })).pipe(gulp.dest(listManagementRoot));

    //del(listManFiles, { force: true });

    var socialsFiles = [
        socialRoot + "Sitecore.Social.Azure.Index.Master.config.disabled",
        socialRoot + "Sitecore.Social.Azure.Index.Web.config.disabled",
        socialRoot + "Sitecore.Social.Azure.IndexConfiguration.config.disabled"
    ];

    gulp.src(socialsFiles).pipe(rename(function (path) {
        path.extname = ""; path.basename = path.basename.replace(".disabled");
    })).pipe(gulp.dest(socialRoot));

    //del(socialsFiles, { force: true });
});
