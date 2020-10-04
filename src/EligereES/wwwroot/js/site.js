// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function PollingStationCommissionUI() {
    this.Id = null;
    this.ElectionFk = null;
    this.Location = null;
    this.DigitalLocation = null;
    this.Description = null;
    this.President = null;
}

var apiBaseUrl = "api/v1.0/"

function AddPollingStationComission(psc, ok) {
    $.ajax({
        url: apiBaseUrl + "Election/" + psc.ElectionFK + "/PSCommission",
        type: 'PUT',
        contentType: 'application/json',
        data: JSON.stringify(psc)
    }).done((v) => ok(v));
}