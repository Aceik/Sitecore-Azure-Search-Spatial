# Sitecore Azure Search Geo Spatial 

The repository contains a runnable Sitecore Helix solution [Helix architecture principles](http://helix.sitecore.net). that demonstrates how to achieve relatively fast Geo Spatial searching using the Sitecore Azure Search provider. The solution can be installed using the same tools that the Habitat example site requires. 

The key to this search involves enabling Edm.GeographyPoint and the OData Expression Syntax for Azure Search "$filter=geo.distance(location, geography'POINT(Long Lat)') le Radius "

For getting started, please check out the [Habitat Wiki](../../wiki).  
For more information on **Helix**, please go to [helix.sitecore.net](http://helix.sitecore.net).
For more information on **Sitecore Azure Search overview**, please go to [the documentation](https://doc.sitecore.net/sitecore_experience_platform/setting_up_and_maintaining/search_and_indexing/sitecore_azure_search_overview).

Credits: 
In coming up with a solution we found other these resources helpful: 
1) Sitecore Solr spatial : https://github.com/ehabelgindy/sitecore-solr-spatial
2) Sitecore Lucene spatial: https://github.com/aokour/Sitecore.ContentSearch.Spatial

