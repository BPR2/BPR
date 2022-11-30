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

function getMap(fieldId,long, lat) {
    latlng = new google.maps.LatLng(lat, long);
    var options = {
        zoom: 18, center: latlng,
        mapTypeId: 'satellite'
    };

    map = new google.maps.Map(document.getElementById(fieldId), options);

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

function showPassword() {
    const passElement = document.getElementById('txtPass');

    if (passElement.getAttribute("Type") == "password") {
        passElement.setAttribute("Type", "text");
    }
    else {
        passElement.setAttribute("Type", "password");
    }
}

/* tmp values used for resizing charts (Making charts responsive)*/
var tmpDatas;
var tmpSensors;

function getCharts(datas, sensors)
{
    tmpDatas = datas;
    tmpSensors = sensors;
    google.charts.load('current', { 'packages': ['line', 'corechart'] });
    google.charts.setOnLoadCallback(drawChart);
    google.charts.setOnLoadCallback(drawChart2);

    function drawChart() {
        var chartDiv = document.getElementById('chart_div');
        var data = new google.visualization.DataTable();
        data.addColumn('date', '');
        for (var i = 0; i < sensors.length; i++) {
            data.addColumn('number', sensors[i].TagNumber);
        }

        const arr = [];
        for (var i = 0; i < datas.length; i++) {
            var dt = new Date(datas[i].date)
            arr.push(new Date(dt.getUTCFullYear(), dt.getUTCMonth(), dt.getUTCDate(), dt.getUTCHours(), dt.getUTCMinutes()))
            for (var j = 0; j < datas[i].measurements.length; j++) {
                arr.push(datas[i].measurements[j].temperature)
            }
            data.addRow(arr);
            arr.length = 0;
        }

        var materialOptions = {
            chart: {
                title: 'Temperature of soil °C'
            },
            height: 500
        };

        var materialChart = new google.charts.Line(chartDiv);
        materialChart.draw(data, materialOptions);
    }

    function drawChart2() {
        var chartDiv = document.getElementById('chart_div2');
        var data = new google.visualization.DataTable();
        data.addColumn('date', '');
        for (var i = 0; i < sensors.length; i++) {
            data.addColumn('number', sensors[i].TagNumber);
        }

        const arr = [];
        for (var i = 0; i < datas.length; i++) {
            var dt = new Date(datas[i].date)
            arr.push(new Date(dt.getUTCFullYear(), dt.getUTCMonth(), dt.getUTCDate(), dt.getUTCHours(), dt.getUTCMinutes()))
            for (var j = 0; j < datas[i].measurements.length; j++) {
                arr.push(datas[i].measurements[j].humidity)
            }
            data.addRow(arr);
            arr.length = 0;
        }

        var materialOptions = {
            chart: {
                title: 'Humidity of soil %'
            },
            height: 500
        };

        var materialChart = new google.charts.Line(chartDiv);
        materialChart.draw(data, materialOptions);
    }
}

$(window).resize(function () {
    if (tmpDatas != null && tmpSensors != null)
        getCharts(tmpDatas, tmpSensors);
});

