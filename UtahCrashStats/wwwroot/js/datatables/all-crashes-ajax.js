$(document).ready(function () {
    var options = {};
    options.url = "https://jsonplaceholder.typicode.com/todos/1";
    options.type = "GET";
    options.dataType = "json";
    options.success = function (data) {
        var x = JSON.stringify(data);
        console.log(x);
    };
    options.error = function () {
        $("#msg").html("Error while 
making Ajax call!");
    };
    $.ajax(options);
});