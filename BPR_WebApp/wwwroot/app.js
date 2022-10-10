function expandCollapseField(button, long, lat) {
    var icon = button.querySelector("span").querySelector("i")
    if (icon.classList.contains("fa-angle-down")) {
        icon.classList.remove("fa-angle-down")
        icon.classList.add("fa-angle-up")

        var latlng = new google.maps.LatLng(long, lat);
        var options = {
            zoom: 2, center: latlng,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        };
        var map = new google.maps.Map(document.getElementById("map"), options);

    }else if (icon.classList.contains("fa-angle-up")) {
        icon.classList.remove("fa-angle-up")
        icon.classList.add("fa-angle-down")

    }
}