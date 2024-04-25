function mostrarHora() {
    var fecha = new Date();
    var hora = fecha.getHours();
    var minuto = fecha.getMinutes();
    var segundo = fecha.getSeconds();


    hora = (hora < 10) ? '0' + hora : hora;
    minuto = (minuto < 10) ? '0' + minuto : minuto;
    segundo = (segundo < 10) ? '0' + segundo : segundo;

    document.getElementById('hora').innerHTML = hora;
    document.getElementById('minuto').innerHTML = minuto;
    document.getElementById('segundo').innerHTML = segundo;
}

setInterval(mostrarHora, 1000);
