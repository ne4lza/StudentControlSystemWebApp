"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/dashboardHub").build();

$(function () {
    connection.start().then(function () {
        alert('Sunucuyla Bağlantı Kuruldu');
        // Sayfa yüklendiğinde otomatik olarak öğrenci verilerini getir
        InvokeStudents();
    }).catch(function (err) {
        alert('Error connecting to dashboardHub: ' + err.toString());
    });
});

// Sunucudan öğrenci verilerini getirmek için SignalR isteği yap
function InvokeStudents() {
    // Bağlantının durumunu kontrol et ve yalnızca "Connected" durumdaysa mesaj gönder
    if (connection.state === signalR.HubConnectionState.Connected) {
        connection.invoke("SendProducts").catch(function (err) {
            alert(err.toString());
        });
    } else {
        alert('Connection is not in the "Connected" state.');
    }
}

// TBL_MESSAGE tablosunda değişiklik olduğunda öğrenci verilerini güncelleme işlemi
connection.on("ReceivedProducts", function (students) {
    updateStudentInfo(students);
});

// Öğrenci verilerini güncelleme işlemi
function updateStudentInfo(students) {
    if (students.length >= 2) {
        var student = students[0];
        var student2 = students[1];

        // İlk kart için öğrenci bilgilerini güncelle
        $('#studentImage1').attr('src', student.image);
        $('#studentName1').text(student.studentNameSurName);
        $('#studentClass1').text(student.studentClass);
        $('#guardianName1').text(student.guardianNameSurName);

        // İkinci kart için öğrenci bilgilerini güncelle
        $('#studentImage2').attr('src', student2.image);
        $('#studentName2').text(student2.studentNameSurName);
        $('#studentClass2').text(student2.studentClass);
        $('#guardianName2').text(student2.guardianNameSurName);
    } else {
        console.error("Veri eksik. İkinci öğrenci verisi bulunamadı.");
    }
}


