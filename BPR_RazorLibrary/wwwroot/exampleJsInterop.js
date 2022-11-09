// This is a JavaScript module that is loaded on demand. It can export any number of
// functions, and may import other JavaScript modules if required.

export function showPrompt(message) {
    return prompt(message, 'Type anything here');
}


export function showPassword() {
    ckbox = $('#ckShowPass')
    txtBox = $('#txtPass')

    if (ckbox.is(':checked')) {
        txtBox.attr("Type", "Text");
    }
    else {
        txtBox.attr("Type", "Password");
    }
}

