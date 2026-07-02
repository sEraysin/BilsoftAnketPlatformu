-- Bilsoft Anket Platformu test verisi notları
-- Identity rolleri, test kullanıcıları, personeller ve Ayse -> Ayse Demir eşleştirmesi
-- ApplicationDbContext.OnModelCreating içindeki HasData ile eklenir.

-- Mevcut anket cevaplarını test personellerine dağıtmak için:
UPDATE anket_musteri SET personelID = 1 WHERE servisID % 3 = 1;
UPDATE anket_musteri SET personelID = 2 WHERE servisID % 3 = 2;
UPDATE anket_musteri SET personelID = 3 WHERE servisID % 3 = 0;

UPDATE anket_cevaplar SET personelID = 1 WHERE cevapID % 3 = 1;
UPDATE anket_cevaplar SET personelID = 2 WHERE cevapID % 3 = 2;
UPDATE anket_cevaplar SET personelID = 3 WHERE cevapID % 3 = 0;
