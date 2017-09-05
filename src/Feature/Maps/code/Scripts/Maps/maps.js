(function (window, $, google) {
    "use strict";

    window.MapModule = window.MapModule || {};
    window.MapModule.markerClusters = [];

    $(window.document).ready(function () {
        var isPageEditor = $("body.pagemode-edit");
        if (isPageEditor.length)
            return;
        //loadMapJsScript();
        Aceik.init();
    });

    window.MapModule.initMaps = function () {
        initMapContainers();
    };
    window.MapModule.zoomToMapPoint = function (mapId, latitude, longitude) {
        var map = window.MapModule.getMap(mapId);
        if (map) {
            map.zoomToMapPoint(new google.maps.LatLng(latitude, longitude), 16);
        }
    };
    window.MapModule.getMap = function (mapId) {
        var mapFound;
        $.each(window.MapModule.markerClusters,
            function (index, markerCluster) {
                if (markerCluster.map.Id == mapId) {
                    mapFound = markerCluster.map;
                    return false;
                }
            });

        return mapFound;
    };

    function loadMapJsScript() {
        var scriptTag = $('script[data-key="gmapapi"]');
        if (scriptTag.length == 0) {
            var fileref = window.document.createElement("script");
            fileref.setAttribute("type", "text/javascript");
            fileref.setAttribute("src", "https://maps.googleapis.com/maps/api/js?callback=MapModule.initMaps");
            fileref.setAttribute("async", "");
            fileref.setAttribute("defer", "");
            fileref.setAttribute("data-key", "gmapapi");

            window.document.getElementsByTagName("head")[0].appendChild(fileref);
        }
    }

    function initMapContainers() {
        setMapPrototypes();
        var $elements = $(".map-canvas");
        $.each($elements,
            function (index, element) {
                var $element = $(element);
                var myLatlng = new google.maps.LatLng(-25.274398, 133.775136);
                var mapProperties = {
                    center: myLatlng,
                    zoom: 4,
                    mapTypeId: google.maps.MapTypeId.ROADMAP,
                    zoomControl: true,
                    mapTypeControl: true,
                    scaleControl: true,
                    //streetViewControl: true,
                    //rotateControl: true,
                    //centerMapControl: true
                };
                // var $renderingParamsEl = $element.siblings('input[id="mapRenderingParameters"]');
                // var renderingParams = {};
                // if ($renderingParamsEl) {
                // renderingParams = eval("(" + $renderingParamsEl.val() + ")");
                // setMapProperties(mapProperties, renderingParams);
                // }

                Aceik.map = new google.maps.Map(element, mapProperties);
                //assign unique id to map instance
                Aceik.map.Id = Date.now();
                //Aceik.map.setCustomProperties(renderingParams);
                //render custom controls if any
                //Aceik.map.renderCustomControls();
                //Aceik.map.setDefaultView(mapProperties.center, mapProperties.zoom);
            });
    }

    function setMapPrototypes() {
        google.maps.Map.prototype.zoomToMapPoint = function (latlng, zoom) {
            this.setCenter(latlng);
            this.setZoom(zoom);
        };
        google.maps.Map.prototype.setDefaultView = function (latlng, zoom) {
            this.defaultCenter = latlng;
            this.defaultZoom = zoom;
        };
        google.maps.Map.prototype.resetToDefaultView = function (scope) {
            var $this = scope || this;
            $this.zoomToMapPoint($this.defaultCenter, $this.defaultZoom);
        };
        google.maps.Map.prototype.renderCustomControls = function () {
            // setCustomProperties() has to be called beforehand
            if (this.centerMapControl) {
                var centerMapControl = new CenterMapControl(this.resetToDefaultView, this);
                this.controls[google.maps.ControlPosition.TOP_CENTER].push(centerMapControl);
            }
        };
        google.maps.Map.prototype.setCustomProperties = function (properties) {
            this.centerMapControl = properties.EnableCenterMapControl;
        };
    }

    function setMapProperties(mapProperties, renderingParams) {
        if (renderingParams) {
            if (renderingParams.CenterLocation) {
                mapProperties.center = parseCoordinate(renderingParams.CenterLocation);
            }
            if (renderingParams.ZoomLevel) {
                var zoomLevel = parseInt(renderingParams.ZoomLevel);
                if (zoomLevel < 1)
                    zoomLevel = 1;
                if (zoomLevel > 21)
                    zoomLevel = 21;
                mapProperties.zoom = zoomLevel;
            }
            mapProperties.zoomControl = getCheckboxBooleanValue(renderingParams.EnableZoomControl);
            mapProperties.mapTypeControl = getCheckboxBooleanValue(renderingParams.EnableMapTypeControl);
            mapProperties.scaleControl = getCheckboxBooleanValue(renderingParams.EnableScaleControl);
            mapProperties.streetViewControl = getCheckboxBooleanValue(renderingParams.EnableStreetViewControl);
            mapProperties.rotateControl = getCheckboxBooleanValue(renderingParams.EnableRotateControl);
            mapProperties.MapTypeId = renderingParams.MapType;
        }

        return renderingParams;
    }

    function getCheckboxBooleanValue(value) {
        return value == "1" ? true : false;
    }

    function parseCoordinate(latlngLiteral) {
        if (latlngLiteral && latlngLiteral.split(",").length === 2) {
            var coordinates = latlngLiteral.split(",");
            var latitude = parseFloat(coordinates[0]);
            var longitude = parseFloat(coordinates[1]);

            return new google.maps.LatLng(latitude, longitude);
        }

        return null;
    }

    /*map custom controls*/
    function CenterMapControl(clickHandler, scope) {
        var $this = scope;
        var controlDiv = document.createElement("div");
        controlDiv.style.margin = "10px";

        // Set CSS for the control border.
        var controlUI = document.createElement("div");
        controlUI.style.backgroundColor = "#fff";
        controlUI.style.border = "2px solid #fff";
        controlUI.style.borderRadius = "3px";
        controlUI.style.boxShadow = "0 2px 6px rgba(0,0,0,.3)";
        controlUI.style.cursor = "pointer";
        controlUI.style.marginBottom = "22px";
        controlUI.style.textAlign = "center";
        controlUI.title = "Click to recenter the map";
        controlDiv.appendChild(controlUI);
        controlDiv.index = 1;

        // Set CSS for the control interior.
        var controlText = document.createElement("div");
        controlText.style.color = "rgb(25,25,25)";
        controlText.style.fontFamily = "Roboto,Arial,sans-serif";
        controlText.style.fontSize = "11px";
        controlText.style.lineHeight = "28px";
        controlText.style.paddingLeft = "5px";
        controlText.style.paddingRight = "5px";
        controlText.innerHTML = "Center Map";
        controlUI.appendChild(controlText);

        // Setup the click event listeners: 
        controlUI.addEventListener("click",
            function () {
                clickHandler($this);
            });

        return controlDiv;
    }


    var Aceik = {
        isMapJsScriptLoaded: false,
        map: null,
        markers: [],
        lastSearchLat: null,
        lastSearchLong: null,
        userMarker: null,
        init: function () {
            //$(".map-canvas").hide();
            Aceik.spinUpCheck();

            $("#addressSearch").click(function () {
                var addressLookupFinished = function () {
                    Aceik.lastSearchLat = $("#addressLookupLat").data("latitude");
                    Aceik.lastSearchLong = $("#addressLookupLong").data("longitude");
                    Aceik.performSearch($("#addressLookupLat").data("latitude"), $("#addressLookupLong").data("longitude"));
                };
                Aceik.addressLookup(addressLookupFinished);
                //$(".map-canvas").show();
            });
            $("#latLongSearch").click(function () {
                Aceik.lastSearchLat = $("#lat").val();
                Aceik.lastSearchLong = $("#long").val();
                Aceik.performSearch($("#lat").val(), $("#long").val());
                //$(".map-canvas").show();
            });

            $(".example-searches").find("a").click(function () {
                Aceik.lastSearchLat = $(this).data("lat");
                Aceik.lastSearchLong = $(this).data("long");
                Aceik.performSearch(Aceik.lastSearchLat, Aceik.lastSearchLong);
            });
        },
        addressLookup: function (callback) {

            var addressVal = $("#address").val();
            if (addressVal == "") {
                alert("Please provide a value to search with !");
                return;
            }

            $.ajax(
                {
                    url: "/api/sitecore/Maps/GetAddressLocation",
                    method: "POST",
                    data: {
                        searchAddress: addressVal
                    },
                    success: function (data) {
                        if (data.success && data.data) {
                            $("#addressLookupLat").html("Latitude: " + data.data.Latitude);
                            $("#addressLookupLat").data("latitude", data.data.Latitude);
                            $("#addressLookupLong").html("Longitude: " + data.data.Longitude);
                            $("#addressLookupLong").data("longitude", data.data.Longitude);
                            callback();
                        } else {
                            alert("No location found for that address");
                        }
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        alert("No location found for that address");
                    }
                });
        },
        spinUpCheck: function () {
            if (!Aceik.isMapJsScriptLoaded) {
                loadMapJsScript();
                Aceik.isMapJsScriptLoaded = true;
            }
        },
        performSearch: function (lat, long) {

            if (!Aceik.map) {
                return;
            }

            Aceik.clearMarkers();

            $.ajax(
                {
                    url: "/api/sitecore/Maps/GetMapPoints",
                    method: "POST",
                    data: {
                        lat: lat,
                        longitude: long,
                        radius: $("#radius").val(),
                        maxResults: $("#max").val()
                    },
                    success: function (data) {

                        $("#searchTime").html("Search Time (milliseconds): " + data.SearchMilliseconds);

                        if (data.Results.length == 1) {
                            var marker = getMarker(Aceik.map, data.Results[0]);
                            Aceik.setMarks([marker]);
                            // Aceik.map.setCenter(parseCoordinate(data[0].Location));
                        } else {
                            var markers = [];
                            $.each(data.Results,
                                function (index, mapPoint) {
                                    var marker = Aceik.getMarker(mapPoint);
                                    markers.push(marker);
                                });
                            Aceik.setMarks(markers);
                        }

                        Aceik.addUserMarker()
                        //Aceik.map.setCenter(new google.maps.LatLng(Aceik.lastSearchLat, Aceik.lastSearchLong));
                        Aceik.map.zoomToMapPoint(new google.maps.LatLng(Aceik.lastSearchLat, Aceik.lastSearchLong), 8);

                        $("#searchResults").html("Results: " + data.Results.length);

                        Aceik.loadResultsListing(data.Results);
                    }
                });


        },
        loadResultsListing: function (data) {
            $("#resultsContainer").empty();
            var source = $('#resultsTemplate').html();
            var template = Handlebars.compile(source);

            var arrayLength = data.length;

            if (arrayLength > 100)
                arrayLength = 50;

            for (var i = 0; i < arrayLength; i++) {
                var html = template(data[i]);
                $("#resultsContainer").append(html);
            }
        },
        setMarks: function (markers) {
            var markerCluster = new MarkerClusterer(Aceik.map, markers);
            window.MapModule.markerClusters.push(markerCluster);
            Aceik.markers = markers;
        },
        getMarker: function (mapPoint) {
            var latlng = new google.maps.LatLng(mapPoint.GeoLocation.Coordinates[1], mapPoint.GeoLocation.Coordinates[0]);
            if (latlng) {
                var marker = new google.maps.Marker({
                    position: latlng,
                    title: mapPoint.PlaceName,
                    icon: "http://maps.google.com/mapfiles/kml/pal4/icon56.png"
                });

                var contentString =
                    "<div class='text-primary'>" +
                    "<h2>" + mapPoint.PlaceName + "</h2>" +
                    "<p>" + mapPoint.GeoLocation.Coordinates[0] + ", " + mapPoint.GeoLocation.Coordinates[1] + "</p>" +
                    "<a href='javascript:void(0)' onclick='MapModule.zoomToMapPoint(" + Aceik.map.Id + "," + latlng.lat() + "," + latlng.lng() + ")'><span class='glyphicon glyphicon-zoom-in'/></a>"
                    + "</div>";

                google.maps.event.addListener(marker,
                    "click",
                    function () {
                        var infoWindow = new google.maps.InfoWindow({
                            content: contentString
                        });
                        infoWindow.open(Aceik.map, marker);
                    });

                return marker;
            }
        },
        addUserMarker: function () {
            var latlng = new google.maps.LatLng(Aceik.lastSearchLat, Aceik.lastSearchLong);
            if (latlng) {
                Aceik.userMarker = new google.maps.Marker({
                    position: latlng,
                    title: "Your search location",
                    icon: "/Images/map-point-1.png",
                    map: Aceik.map
                });

                var contentString =
                    "<div class='text-primary'>" +
                    "<h2>Search Location</h2>" +
                    "<a href='javascript:void(0)' onclick='MapModule.zoomToMapPoint(" + Aceik.map.Id + "," + latlng.lat() + "," + latlng.lng() + ")'><span class='glyphicon glyphicon-zoom-in'/></a>"
                    + "</div>";

                google.maps.event.addListener(Aceik.userMarker,
                    "click",
                    function () {
                        var infoWindow = new google.maps.InfoWindow({
                            content: contentString
                        });
                        infoWindow.open(Aceik.map, Aceik.userMarker);
                    });
            }
        },
        clearMarkers: function () {

            if (Aceik.userMarker && Aceik.userMarker.setMap)
                Aceik.userMarker.setMap(null);

            for (var i = 0; i < window.MapModule.markerClusters.length; i++) {
                window.MapModule.markerClusters[i].clearMarkers();
            }
            Aceik.markers = [];
        }
    };

})(window, jQuery, google = window.google || {});