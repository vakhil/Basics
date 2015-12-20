<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DrawRoute.aspx.cs" Inherits="DrawRoute" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
<head>
<title>Testpage route planner with directions</title>
  <script type="text/javascript" src="http://maps.google.com/maps/api/js?sensor=false"></script>
    <script type="text/javascript">
        var directionDisplay;
        var directionsService = new google.maps.DirectionsService();
        function initialize() {
            var latlng = new google.maps.LatLng(17.497535, 78.408622);
            directionsDisplay = new google.maps.DirectionsRenderer();
            var myOptions = {
                zoom: 14,
                center: latlng,
                mapTypeId: google.maps.MapTypeId.ROADMAP,
                mapTypeControl: false
            };
            var map = new google.maps.Map(document.getElementById("map_canvas"), myOptions);
            directionsDisplay.setMap(map);
            directionsDisplay.setPanel(document.getElementById("directionsPanel"));
            var marker = new google.maps.Marker({
                position: latlng,
                map: map,
                title: "My location"
            });
        }
        function calcRoute() {
            var start = "17.997535, 78.708622";
            var end = "17.497535, 78.408622";
            var request = {
                origin: start,
                destination: end,
                travelMode: google.maps.DirectionsTravelMode.DRIVING
            };
            directionsService.route(request, function (response, status) {
                if (status == google.maps.DirectionsStatus.OK) {
                    directionsDisplay.setDirections(response);
                }
            });
        }
  </script>
</head>
<body onload="initialize()">
  <div id="map_canvas" style="width:710px; height:300px"></div>   
  <form action="/routebeschrijving" onsubmit="calcRoute();return false;" id="routeForm">
    <input type="text" id="routeStart" value="">
    <input type="submit" value="Calculate route">
  </form>
  <div id="directionsPanel"></div>
</body>
</html>
