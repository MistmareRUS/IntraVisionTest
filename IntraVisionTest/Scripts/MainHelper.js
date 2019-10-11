
function changeFlag(s) {
    var target = document.getElementById(s)
    target.value = target.checked
}

function enableFields() {
    var target = document.getElementById("newDrink")
    target.style.display = 'block';
    location.href='#bottom';
}

function SumsDivUpdate() {
    $.ajax({
        type: "POST",
        url: "/Home/ShowSums",
        data: param = "",
        dataType: "html",
        success: function (data) { $("#counter").html(data) }
    });
}
function InfoDivUpdate() {
    $.ajax({
        type: "POST",
        url: "/Home/ShowInfo",
        data: param = "",
        dataType: "html",
        success: function (data) { $("#infoDiv").html(data) }
    });
}

function errorFunc(errorData) {
    alert('Ошибка' + errorData);
}