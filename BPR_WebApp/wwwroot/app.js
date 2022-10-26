function expandCollapse(button) {
    var icon = button.querySelector("span").querySelector("i")
    if (icon.classList.contains("fa-angle-down")) {
        icon.classList.remove("fa-angle-down")
        icon.classList.add("fa-angle-up")

    }else if (icon.classList.contains("fa-angle-up")) {
        icon.classList.remove("fa-angle-up")
        icon.classList.add("fa-angle-down")

    }
}

function getMap(long, lat) {
    latlng = new google.maps.LatLng(lat, long);
    var options = {
        zoom: 18, center: latlng,
        mapTypeId: 'satellite'
    };

    map = new google.maps.Map(document.getElementById("map"), options);

    new google.maps.Marker({
        position: latlng,
        map: map,
        title: 'Receiver',
        icon: 'pin.png'
    });
}

function disableReadonly() {
    const element = document.getElementById('SensorSerialNumber');
    if (element.readOnly) {
        document.getElementById('SensorSerialNumber').removeAttribute('readonly')
    }
    else {
        document.getElementById('SensorSerialNumber').readOnly = true
    }
}

