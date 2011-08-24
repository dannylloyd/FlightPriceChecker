function sortList(id) {
    var mylist = $('ul#' + id);
    var listitems = mylist.children('li').get();
    listitems.sort(function (a, b) {
        var compA = $(a).text().toUpperCase();
        var compB = $(b).text().toUpperCase();
        return (compA < compB) ? -1 : (compA > compB) ? 1 : 0;
    })
    $.each(listitems, function (idx, itm) { mylist.append(itm); });
}
function setPreviousValues() {
    //$.cookie('settings', settings);
    if ($.cookie('settings') !== null) {
        var settings = JSON.parse($.cookie('settings'))
        $('#ddlOriginAirport').val(settings.departureAirport);
        $('#ddlDestinationAirport').val(settings.arrivalAirport);
        $('#txtDepartureDate').val(settings.departureDate);
        $('#txtReturnDate').val(settings.returnDate);
        $.cookie('settings', JSON.stringify(settings));
    }
}
$(document).ready(function () {
    var dates = $("#txtDepartureDate, #txtReturnDate").datepicker({
        showOtherMonths: false,
        selectOtherMonths: false,
        numberOfMonths: 1
    });

    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1; //January is 0!
    var yyyy = today.getFullYear();
    if (dd < 10) { dd = '0' + dd }
    if (mm < 10) { mm = '0' + mm }

    $('#ddlOriginAirport').val('LIT');
    setPreviousValues();

    $('form input[type=submit]').button();
    if ($('.searchResultsTable').length != 0) {
        $('#results div, #results table, #results ul, #results img, .aboveSubmitButtonText').not('.outboundResultsHeaderWrapper, .submitContainer, table.searchResultsTable div, table.searchResultsTable, #page, .searchResults, #sw_content, #sw_main_section, .select-flights, #sw_main_column_container, #sw_main, #page_content').css({ 'display': 'none' });

        $(".searchResultsTable th").each(function () {
            $(this).addClass("ui-state-default");
        });
        $(".searchResultsTable td").each(function () {
            $(this).addClass("ui-widget-content");
        });
        $("#faresReturn input[type=radio]").change(function () {
            $('#faresReturn tr').each(function () {
                $(this).children("td").removeClass("ui-state-hover");
            });
            if ($(this + ":checked").val()) {
                $(this).parents("tr").children("td").addClass("ui-state-hover");
            }
        });
        $("#faresOutbound input[type=radio]").change(function () {
            $('#faresOutbound tr').each(function () {
                $(this).children("td").removeClass("ui-state-hover");
            });
            if ($(this + ":checked").val()) {
                $(this).parents("tr").children("td").addClass("ui-state-hover");
            }
        });
    }
    else {
        sortList("DepartureFlight");
        sortList("ReturnFlight");
    }

    if ($('#errors').length != 0) {
        $('#errors').css({ 'position': 'absolute', 'top': '320px', 'left': '0px' });
        $('#results').css({ 'margin-top': '3000px' });
    }

    $('#btnSubmit, #btnGoToFare').click(function () {
        var settings = { 'departureAirport': $('#ddlOriginAirport').val(),
            'arrivalAirport': $('#ddlDestinationAirport').val(),
            'departureDate': $('#txtDepartureDate').val(),
            'returnDate': $('#txtReturnDate').val()
        };
        $.cookie('settings', JSON.stringify(settings));
    });
    $('#results').css({ 'display': 'block' });
});