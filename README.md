# Sitecore Azure Search Geo Spatial 

This repository contains a runnable Sitecore Helix solution ([Helix architecture principles](http://helix.sitecore.net)) that demonstrates how to achieve a relatively fast Geo Spatial search using the Sitecore Azure Search provider. The solution can be installed using the same tools that the Habitat example site requires. 

The core functionality is also available as an [installable Sitecore package.] (../../wiki/05-Installing-Sitecore-Package)
A Demo Installation package built from the example helix spatial site can then be installed if you just want to see it in action. 

The key to this search involves enabling Edm.GeographyPoint and the OData Expression Syntax for Azure Search "$filter=geo.distance(location, geography'POINT(Long Lat)') le Radius "

For getting started, please check out the [Wiki](../../wiki).  
For more information on **Helix**, please go to [helix.sitecore.net](http://helix.sitecore.net).
For more information on **Sitecore Azure Search overview**, please go to [the documentation](https://doc.sitecore.net/sitecore_experience_platform/setting_up_and_maintaining/search_and_indexing/sitecore_azure_search_overview).

Credits: 
We found these resources helpful when developing this solution: 
1) Sitecore Solr spatial : https://github.com/ehabelgindy/sitecore-solr-spatial
2) Sitecore Lucene spatial: https://github.com/aokour/Sitecore.ContentSearch.Spatial

Thanks also to other Aceik team members that assisted with this solution along the way.  Ed, Jose, Tony. 

