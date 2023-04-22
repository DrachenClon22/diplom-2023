function ShowPassword(elementId) {
    var x = document.getElementById(elementId);
    if (x.type === "password") {
        x.type = "text";
    } else {
        x.type = "password";
    }
}

function checkFieldsByName(theForm, name1, name2) {
    if (theForm[name1].value != theForm[name2].value) {
        return false;
    } else {
        return true;
    }
}

function changeTextInElement(classElement, text) {
    var div = document.getElementById(classElement);
    div.innerHTML = text;
} 