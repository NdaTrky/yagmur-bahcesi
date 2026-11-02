# Yağmur Bahçesi 0.2 — uygulanan genişleme

2 Kasım 2026. Android için oynanabilir test paketi; ticari yayın yapılmadı. Kullanıcının “Hadi uygulayalım” onayıyla geliştirildi. Test sonuçları [geliştirme raporunda](GELISTIRME_DURUMU.md).

## Döngü ve içerik

Etkinlik seç → tamamla → 1 bahçe pulu kazan → yeni tür aç → bahçeye yerleştir → kalıcı keşif/proje ilerlesin. Pul yalnız oyun içinde kazanılır. Serbest bahçe araçları baştan açıktır. Etkinlikten çıkmak ilerlemeyi silmez; üç oyunun yarım kalan durumu ayrı saklanır. Süre, enerji, günlük seri ve kaybolan ödül yoktur.

| İçerik | Uygulanan davranış |
|---|---|
| Piknik Sepeti | Görsel sıradaki elma/armudu seçme; üç farklı meyve dizisi |
| Bulutun Yolu | 3×3 alanda beş kanalı döndürme; üç ayrı bağlantı rotası |
| Yaprak Kaptanı | 5×3 gölette yön seçme; üç farklı başlangıç/iskele/engel düzeni |
| Yardım | Bir doğru adımı birlikte yapma; ceza veya ödül azaltma yok |
| Çiçek koleksiyonu | 8 tür × 3 renk tonu; 6 dikim alanı. Kozmos/papatya açık; çan çiçeği 1 pul, diğer yeni türler 2 pul |
| Dekor koleksiyonu | 12 nesne; ilk üçü açık. Yenileri 1–3 pul. Açılan nesneyi kaldırma ve tekrar koyma ücretsiz |
| Projeler | Mina, Luli ve Pufi için üçer keşifle gelişen üç bahçe noktası; Sumi için üç keşif; toplam 12 kalıcı işaret |
| Sumi | Bulut oyunu, üç farklı çiçek yetiştirme ve üç mini oyunu keşfetmeyle açılan beşinci dost |
| Albüm | Uygulama alanında son altı bahçe görüntüsü; önceki/sonraki görüntü ve yeni anı düğmeleri |

Çiçekler kelebek, nota, kısa tepki ve dalga davranışlarını paylaşır. Bunlar sekiz ayrı simülasyon veya 24 ayrı mekanik olarak sayılmaz. Sulama kabı bitkiyi büyütür; çan/rüzgârgülü rüzgârı, çeşme dalgayı, sepet paylaşmayı tetikler. Bahçeye eklenen yeni nesneler çalışma sırasında gerçek Unity GameObject/renderer/collider olarak oluşturulur. Kaynak sahnenin mevcut 16 nesnesi korunur; çalışma sırasında toplam 29 etkileşim nesnesi oluşur.

## Kayıt ve ödül güvenilirliği

Kayıt sürümü 2, dosya adı geriye erişim için hâlâ `garden-v1.json`. Sürüm 1 bitkileri, renklerden türe dönüştürülen çiçekler, tekne/piknik/dekor ve ayarlar korunur. İlk göçten önce özgün JSON `.v1-preserved` olarak saklanır; sonraki yazmalar bu dosyayı değiştirmez.

Bakiye ve tür açma tek kayıt işlemidir. Değişiklik önce durumun kopyasında hazırlanır; diske yazılamazsa satın alma ve bakiye canlı duruma uygulanmaz. Ödül etkinlik kimliği ve işlenmiş bayrağıyla tekrar alınamaz. Tamamlanan etkinliğin kaydı ile ödül arasında kapanma olursa sonuç ekranı bekleyen ödülü yeniden dener. Daha yeni bir kayıt sürümüyle karşılaşılırsa dosya yazmaya kapatılır. Bozuk ana kayıt geçerli yedekten kurtarılır.

Unity JsonUtility'nin boş inline nesneler üretmesi, ilk mini oyunu açmayı engelleyen bir hata olarak bulundu. Başlatma ve kopyalama artık boş/geçersiz etkinliği denetliyor; ilk açılış senaryosu otomatik teste eklendi.

Windows otomatik testleri her çalıştırmada ayrı geçici klasör ve albüm kullanır. Android test scripti yalnız `RainGarden_API36` adlı ayrılmış AVD üzerinde çalışır; eski test kaydını kopyaladıktan sonra uygulama verisini sıfırlar. Kullanıcının normal Windows bahçesi test için değiştirilmez.

## Kodun yeri

- `GardenCatalog.cs`: sabit tür kimlikleri, fiyatlar, görsel/davranış eşlemesi.
- `GardenActivities.cs`: üç mini oyunun kuralları, düzenleri, yardım ve tekil ödül.
- `GardenState.cs`: kayıt, göç, doğrulama ve kurtarma.
- `GardenProgression.cs`: bahçeye yerleştirme, proje parçaları, Sumi ve yerel albüm.
- `GardenExpansionUI.cs`: koleksiyon, dikim, keşif, mini oyun ve albüm ekranları.
- `GardenChecks.cs`, `GardenSmoke.cs`, `tools/Test-AndroidGarden.ps1`: kural, masaüstü ve Android doğrulaması.

Yeni bitki/dekor ve Sumi atlasları `Art/Expansion.png`, `Art/Sumi.png`; üretim kayıtları [ART_EXPANSION_PROMPTS.json](ART_EXPANSION_PROMPTS.json). Mevcut sanat dosyaları korundu.

## Yayın öncesi kalan içerik ve değerlendirme

- Türkçe/İngilizce görsel yönerge ve yardım var; konuşmalı yönergeler henüz üretilmedi.
- Piknikteki üç meyve dizisi, önerideki gruplama/serbest sofra gibi üç ayrı kural ailesinin tamamı değildir. Aile testinde anlaşılabilirlik görüldükten sonra bu çeşitlilik tamamlanmalı.
- Ziyaret kartları, özel sırayla oyuncu modu, ayrı bahçe yuvaları, yeni bölgeler, Doğa Orkestrası ve Renk Serası eklenmedi. Aynı cihazı sırayla kullanmak mümkün; ağ üzerinden veya aynı anda iki oyuncu desteği yok.
- Aile pilotu, gerçek ARM64 telefon/tablet ve 16 KB cihaz çalışma testi yapılmadı. Pul dengesi, metinsiz anlaşılabilirlik ve uzun vadeli bağlılık henüz doğrulanmış değil.
- Yayıncı/Play hesabı, ürün kimliği, imzalı AAB ve mağaza süreçleri [yayın planında](YAYIN_HAZIRLIGI.md). Roblox ayrı uyarlamadır; [planı](ROBLOX_UYARLAMA_PLANI.md) hazır, Roblox projesi/yayını henüz yok.

Bu paket yeni döngüyü çalışır biçimde değerlendirmek içindir. Onaylanan tüm alternatiflerin veya yayın kabulünün tamamlandığı anlamına gelmez.
