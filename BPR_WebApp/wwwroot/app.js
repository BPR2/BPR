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

function showPassword() {
    const passElement = document.getElementById('txtPass');

    if (passElement.getAttribute("Type") == "password") {
        passElement.setAttribute("Type", "text");
    }
    else {
        passElement.setAttribute("Type", "password");
    }
}


function getchart(datas, sensors)
{
    var chartData = datas;
    google.charts.load('current', { 'packages': ['line', 'corechart'] });
    google.charts.setOnLoadCallback(drawChart);

    function drawChart() {
        var chartDiv = document.getElementById('chart_div');
        var data = new google.visualization.DataTable();
        data.addColumn('date', 'Day');
        //data.addColumn('number', "Temperature");
        //data.addColumn('number', "Humidity");
        for (var i = 0; i < sensors.length; i++) {
            data.addColumn('number', sensors[i].TagNumber);
        }

        for (var i = 0; i < chartData.length / sensors.length; i + sensors.length) {  
            var dt = new Date(chartData[i].date);
            for (var j = 0; j < chartData[i].measurements.length; j++) {
                var dt2 = new Date("0001-01-01T"+chartData[i].measurements[j].time)
                dt.setHours(dt2.getHours(), dt2.getMinutes(), dt2.getSeconds());
                data.addRow([new Date(dt.getUTCFullYear(), dt.getUTCMonth(), dt.getUTCDate(), dt.getUTCHours(), dt.getUTCMinutes()), chartData[i].measurements[j].temperature]);

            }
        }

        //new Date(dt.getFullYear(), dt.getMonth(), dt.getDay())


        //data.addRows([
        //    [new Date(2022, 0, 1, 1), -.5, 20],
        //    [new Date(2022, 0, 1, 3), .4, 35],
        //    [new Date(2022, 0, 1, 5), .5, 40],
        //    [new Date(2022, 0, 1, 7), 2.9, 45],
        //    [new Date(2022, 0, 1, 9), 6.3, 50],
        //    [new Date(2022, 0, 1, 11), 9, 40],
        //    [new Date(2022, 0, 1, 13), 10.6, 35],
        //    [new Date(2022, 0, 1, 15), 11, 40],
        //    [new Date(2022, 0, 1, 17), 13.6, 45],
        //    [new Date(2022, 0, 1, 19), 12.6, 50],
        //    [new Date(2022, 0, 1, 21), 5.6, 45],
        //    [new Date(2022, 0, 1, 23), 4, 45],
        //    [new Date(2022, 0, 2, 1), 3, 55],
        //    [new Date(2022, 0, 2, 3), .4, 50],
        //    [new Date(2022, 0, 2, 5), .5, 55]
        //]);

        var materialOptions = {
            chart: {
                title: 'Temperature and Humidity of soil'
            },
            width: 900,
            height: 500
            //series: {
            //    // Gives each series an axis name that matches the Y-axis below.
            //    0: { axis: 'Temps' },
            //    1: { axis: 'Humidity' }
            //}
            //axes: {
            //    // Adds labels to each axis; they don't have to match the axis names.
            //    y: {
            //        Temps: { label: 'Temps °C' },
            //        Humidity: { label: 'Humidity %' }
            //    }
            //}
        };

        function drawMaterialChart() {
            var materialChart = new google.charts.Line(chartDiv);
            materialChart.draw(data, materialOptions);
        }

        drawMaterialChart();
    }
}

