$(function () {
    
    $.fn.drawCMISDocInfo = function () {
        $.ajax({
            type: "POST",
            url: "index.aspx/DocumentInfo",
            data: '{}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (data) {
                doc = []; year = [];
                total = 0; avg = [];
                $.each(data.d, function (i, x) {
                    doc.push(x.doc);
                    year.push(x.year);
                    avg.push(x.doc);
                    total = total + x.doc;

                });
            },
            failure: function (response) {
                alert(response.d);
            }
        });
        $('#container').highcharts({
            chart: {
                type: 'column'
            },
            title: {
                text: 'د کال په اساس د پایلو جدولونو شمیر'
            },
            subtitle: {
                text: 'Uploaded Resultbooks by Year'
            },
            xAxis: {
                categories: year,
                crosshair: true

            },
            legend: {
                layout: 'vertical',
                align: 'right',
                verticalAlign: 'middle',
                borderWidth: 0
            },
            tooltip: {
                headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                    '<td style="padding:0"><b>{point.y:.1f} </b></td></tr>',
                footerFormat: '</table>',
                shared: true,
                useHTML: true
            },
            yAxis: {
                min: 0, labels: {
                    formatter: function () {
                        return this.value;
                    }
                },
                title: {
                    text: 'د پایلو جدولونو شمیر'
                },
                plotLines: [{
                    value: 0,
                    width: 1,
                    color: '#808080'
                }]
            },
            series: [{
                type: 'column',
                name: 'جدول',
                data: doc
            }, {
                name: 'ResulBooks',
                type: 'spline',
                data: avg,
                tooltip: {
                    valueSuffix: 'B'
                }, marker: {
                    lineWidth: 2,
                    lineColor: Highcharts.getOptions().colors[3],
                    fillColor: 'white'
                }
            },
                {
                type: 'pie',
                allowPointSelect: true,
                name: 'ټول پورته شوی جدولونه',
                data: [{
                    name: 'ټول جدولونه',
                    y: total,
                    sliced: true,
                    selected: true,
                    color: Highcharts.getOptions().colors[0]
                }],
                center: [100, 0],
                size: 100,
                showInLegend: false,
                dataLabels: {
                    enabled: true
                }
            }]
        });
    }



    $.fn.drawCMISStudentInfo = function () {
        $.ajax({
            type: "POST",
            url: "index.aspx/StudentStatisticInfo",
            data: '{}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (data) {
                male = []; female = []; total = []; year = [];
                Tmale = 0; Tfemale = 0;
                $.each(data.d, function (i, x) {
                    male.push(x.male);
                    female.push(x.female);
                    total.push(x.total);
                    year.push(x.year);                    
                    Tmale = Tmale + x.male;
                    Tfemale = Tfemale + x.female;
                });
            },
            failure: function (response) {
                alert(response.d);
            }
        });
        $('#container').highcharts({
            chart: {
                type: 'column'
            },
            title: {
                text: 'د فارغ شویو زده کوونکو احصاییه'
            },
            subtitle: {
                text: 'Graduted Students Statistics'
            },
            xAxis: {
                categories: year,
                crosshair: true
                
            },
            legend: {
                layout: 'vertical',
                align: 'right',
                verticalAlign: 'middle',
                borderWidth: 0
            },
                tooltip: {
            headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
            pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                '<td style="padding:0"><b>{point.y:.1f} </b></td></tr>',
            footerFormat: '</table>',
            shared: true,
            useHTML: true
                },
            yAxis: {
                min: 0, labels: {
                    formatter: function () {
                        return this.value;
                    }
                },
                title: {
                    text: 'د زده کوونکو شمیر'
                },
                plotLines: [{
                    value: 0,
                    width: 1,
                    color: '#808080'
                }]
            },
            series: [{
                type: 'column',
                name: 'نارینه',
                data: male
            }, {
                type: 'column',
                name: 'ښځینه',
                data: female
            }, {
                type: 'column',
                name: 'ټول',
                data: total
            }, {
                type: 'pie',
                allowPointSelect: true,
                name: 'ټول فارغ شوی زده کوونکی',
                data: [{
                    name: 'نارینه',
                    y: Tmale,
                    sliced: true,
                    selected: true,
                    color: Highcharts.getOptions().colors[0]
                }, {
                    name: 'ښځینه',
                    y: Tfemale,
                    color: Highcharts.getOptions().colors[1]
                }],
                center: [100,0],
                size: 100,
                showInLegend: false,
                dataLabels: {
                    enabled: true
                }
            }]
        });
    }

});