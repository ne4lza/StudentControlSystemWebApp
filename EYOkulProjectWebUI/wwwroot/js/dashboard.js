"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/dashboardHub").build();

$(function () {
    connection.start().then(function () {
        console.log('Connected to dashboardHub');
        // Sayfa yüklendiğinde otomatik olarak öğrenci verilerini getir
        InvokeStudents();
    }).catch(function (err) {
        console.error('Error connecting to dashboardHub: ' + err.toString());
    });
});

// Sunucudan öğrenci verilerini getirmek için SignalR isteği yap
function InvokeStudents() {
    // Bağlantının durumunu kontrol et ve yalnızca "Connected" durumdaysa mesaj gönder
    if (connection.state === signalR.HubConnectionState.Connected) {
        connection.invoke("SendProducts").catch(function (err) {
            console.error(err.toString());
        });
    } else {
        console.error('Connection is not in the "Connected" state.');
    }
}

// TBL_MESSAGE tablosunda değişiklik olduğunda öğrenci verilerini güncelleme işlemi
connection.on("ReceivedProducts", function (students) {
    updateStudentInfo(students);
});

// Öğrenci verilerini güncelleme işlemi
function updateStudentInfo(students) {
    if (students.length > 0) {
        var student = students[0]; // İlk öğrenciyi seçelim, burada tüm öğrencilerin listesi yer alacaksa döngü kullanılabilir

        // Öğrenci bilgilerini güncelle
        $('#studentImage').attr('src', student.image);
        $('#studentName').text(student.studentNameSurName);
        $('#studentClass').text(student.studentClass);
        $('#guardianName').text(student.guardianNameSurName);
        // Diğer bilgileri de güncelleyebilirsiniz
    }
}
