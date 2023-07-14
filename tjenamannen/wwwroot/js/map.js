var map = new ol.Map({
    
    target: 'map', // The ID of the HTML element where the map should be rendered
    layers: [
        new ol.layer.Tile({
            source: new ol.source.OSM() // Use OpenStreetMap as the base layer
        })
    ],
    view: new ol.View({
        center: ol.proj.fromLonLat([0, 0]), // Set the initial map center (longitude, latitude)
        zoom: 10 // Set the initial zoom level
    })

});
