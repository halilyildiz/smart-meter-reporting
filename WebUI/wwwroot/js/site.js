
function GetLastMeterData() {
  var serialNumber = $('#searchSerialNumber').val();
  $.ajax({
    url: '/Meter/GetLastMeterData',
    type: 'GET',
    data: { serialNumber: serialNumber },
    success: function (response) {
      $('#MeterTableBody').html(response);
    },
    error: function (xhr) {
      alert('Veri bulunamadı: ' + xhr.responseText);
    }
  });
}

function RequestReport() {
  var serialNumber = $('#searchSerialNumber').val();
  $.ajax({
    url: '/Report/RequestNewReport',
    type: 'POST',
    data: { serialNumber: serialNumber },
    success: function (response) {
      alert(response);
    },
    error: function (xhr) {
      alert('Yeni rapor talebi başarısız: ' + xhr.responseText);
    }
  });
}

function SaveMeterData() {
  var meterRequest = {
    MeterSerialNumber: $('#serialNumber').val(),
    LastIndex: $('#lastIndex').val(),
    Current: $('#current').val(),
    Voltage: $('#voltage').val(),
    MeasurementTime: $('#measurementTime').val()
  };

  $.ajax({
    url: '/Meter/SaveMeterData',
    type: 'POST',
    contentType: 'application/json', 
    data: JSON.stringify(meterRequest), 
    success: function (response) {
      $('#MeterTableBody').html(response); 
      $("#exampleModal").modal("hide")
    },
    error: function (xhr) {
      alert('Veri kaydedilemedi: ' + xhr.responseText);
    }
  });
}

function DownloadReport(reportId) {
  $.ajax({
    url: '/Report/DownloadReport',
    type: 'GET',
    data: { reportId: reportId },
    xhrFields: {
      responseType: 'blob'
    },
    success: function (blob, status, xhr) {
      var fileName = "rapor.txt"
      var link = document.createElement('a');
      link.href = window.URL.createObjectURL(blob);
      link.download = fileName;
      link.click();
    },
    error: function (xhr) {
      alert('Rapor indirilemedi: ' + xhr.responseText);
    }
  });
}
