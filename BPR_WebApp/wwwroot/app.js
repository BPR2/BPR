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


function getchart(datas)
{
    google.charts.load('current', { 'packages': ['line', 'corechart'] });
    google.charts.setOnLoadCallback(drawChart);

    function drawChart() {
        var chartDiv = document.getElementById('chart_div');
        var data = new google.visualization.DataTable();
        data.addColumn('date', 'Month');
        data.addColumn('number', "Temperature");
        data.addColumn('number', "Humidity");

        data.addRows([
            [new Date(2022, 0, 1, 1), -.5, 20],
            [new Date(2022, 0, 1, 3), .4, 35],
            [new Date(2022, 0, 1, 5), .5, 40],
            [new Date(2022, 0, 1, 7), 2.9, 45],
            [new Date(2022, 0, 1, 9), 6.3, 50],
            [new Date(2022, 0, 1, 11), 9, 40],
            [new Date(2022, 0, 1, 13), 10.6, 35],
            [new Date(2022, 0, 1, 15), 11, 40],
            [new Date(2022, 0, 1, 17), 13.6, 45],
            [new Date(2022, 0, 1, 19), 12.6, 50],
            [new Date(2022, 0, 1, 21), 5.6, 45],
            [new Date(2022, 0, 1, 23), 4, 45],
            [new Date(2022, 0, 2, 1), 3, 55],
            [new Date(2022, 0, 2, 3), .4, 50],
            [new Date(2022, 0, 2, 5), .5, 55],
            [new Date(2022, 0, 2, 7), 2.9, 45],
            [new Date(2022, 0, 2, 9), 6.3, 35],
            [new Date(2022, 0, 2, 11), 9, 40],
            [new Date(2022, 0, 2, 13), 10.6, 40],
            [new Date(2022, 0, 2, 15), 12, 50],
            [new Date(2022, 0, 2, 17), 10, 55],
            [new Date(2022, 0, 2, 19), 8, 53],
            [new Date(2022, 0, 2, 21), 6, 56],
            [new Date(2022, 0, 2, 23), 4, 45],
            [new Date(2022, 0, 3, 1), 1, 48],
            [new Date(2022, 0, 3, 3), .4, 46],
            [new Date(2022, 0, 3, 5), .5, 43],
            [new Date(2022, 0, 3, 7), 2.9, 44],
            [new Date(2022, 0, 3, 9), 6.3, 46],
            [new Date(2022, 0, 3, 11), 9, 55],
            [new Date(2022, 0, 3, 13), 10.6, 56],
            [new Date(2022, 0, 3, 15), 10.6, 55],
            [new Date(2022, 0, 3, 17), 10.6, 50],
            [new Date(2022, 0, 3, 19), 10.6, 45],
            [new Date(2022, 0, 3, 21), 10.6, 35],
            [new Date(2022, 0, 3, 23), 4, 45]
        ]);

        var materialOptions = {
            chart: {
                title: 'Temperature and Humidity of soil'
            },
            width: 900,
            height: 500,
            series: {
                // Gives each series an axis name that matches the Y-axis below.
                0: { axis: 'Temps' },
                1: { axis: 'Humidity' }
            },
            axes: {
                // Adds labels to each axis; they don't have to match the axis names.
                y: {
                    Temps: { label: 'Temps °C' },
                    Humidity: { label: 'Humidity %' }
                }
            }
        };

        function drawMaterialChart() {
            var materialChart = new google.charts.Line(chartDiv);
            materialChart.draw(data, materialOptions);
        }

        drawMaterialChart();
    }
}

