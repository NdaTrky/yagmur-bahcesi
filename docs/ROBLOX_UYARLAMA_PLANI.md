# Yağmur Bahçesi — Roblox uyarlaması

Araştırma tarihi: 2 Kasım 2026. Bu belge bir uyarlama planıdır; Roblox projesi oluşturulmadı ve Roblox'ta yayın yapılmadı. Android geliştirmesi devam ediyor.

**Evet, aynı konsepti Roblox'ta yayınlamak mümkün.** Unity APK'sı veya C# projesi Roblox'a doğrudan yüklenemez. Roblox Studio içinde sahne, arayüz ve oyun mantığı yeniden kurulur; dil Luau'dur. Resim, ses, karakter tasarımları ve oyun kuralları yeniden kullanılabilir. [Roblox'un Unity geliştiricileri için rehberi](https://create.roblox.com/docs/unity)

## Uygulama seçenekleri

| Yol | Yeniden kullanılabilecekler | Yeni iş | Değerlendirme |
|---|---|---|---|
| Sabit kameralı 2D bahçe | Mevcut arka plan, sprite'lar, mini oyun resimleri | ScreenGui, dokunma/sürükleme, Luau kuralları, kayıt | En küçük teknik deneme; mevcut görüntüye yakın |
| Küçük 3D bahçe, resimli mini oyun ekranları | Karakter kimliği, koleksiyon, ödül kuralları ve UI resimleri | Basit 3D bahçe/çiçek/dekor, avatar hareketi, kamera, sunucu mantığı | Roblox için önerilen ürün yönü; daha fazla sanat ve test işi |
| Ortak çok oyunculu bahçe | Konsept ve içerik tasarımı | Eşzamanlı düzenleme, izinler, bağlantı kopması, sunucu testleri | İlk denemeden sonra; bakım yükü en yüksek seçenek |

Önerim: önce tek bahçe + Piknik Sepeti + bir pul + çan çiçeği dikimi + çıkış/girişte kayıt döngüsünü özel bir Studio denemesinde doğrulamak. Sonra 3D bahçe ve diğer iki mini oyun için karar vermek. Bu aşamada açık sohbet, takas ve rastgele ücretli ödüller tasarıma eklenmez.

## Teknik iş listesi

1. Windows'ta Roblox Studio ve kullanıcıya ait geliştirici hesabıyla özel bir deneyim oluşturulur. Hesaba giriş ve kimlik doğrulaması hesap sahibinde kalır; parola veya kimlik belgesi projeye kaydedilmez.
2. Android projesinden ayrı bir `Roblox/` kaynak dizini hazırlanır. Sabit içerik kimlikleri korunur; `Catalog`, `Activities`, `Projects` Luau modülleri olur. Unity sahne dosyaları ve C# kodu çalışır Roblox kodu sayılmaz.
3. PNG/WAV varlıkları uygun sahiplik altında yüklenir; oluşan asset ID'leri yapılandırmaya yazılır. Moderasyon ve kullanım izinleri kontrol edilir. 3D yol seçilirse yeni modeller gerekir.
4. İstemci yalnızca oyuncunun eylemini gönderir. Sunucu hedefi, sahipliği, bakiyeyi, etkinlik sırasını ve ödül kimliğini doğrular. İstemcinin gönderdiği “100 pul kazandım” değerine güvenilmez.
5. Yerel JSON yerine sunucu tarafında oyuncuya bağlı DataStore kaydı kurulur. `UpdateAsync`, kayıt sürümü, hata sonrası yeniden deneme ve yeniden bağlanma senaryoları ele alınır. Unity kaydı kendiliğinden Roblox'a taşınmaz; iki ürünün ilerlemesi başlangıçta ayrıdır. DataStore erişimi sunucu Script'leri üzerinden yapılır. [Resmî DataStore belgesi](https://create.roblox.com/docs/cloud-services/data-stores)
6. Studio'da istemci/sunucu, telefon/tablet oranları, iki test oyuncusu, ağ kesilmesi ve ödül tekrarı sınanır. Ardından gerçek Roblox istemcilerinde test edilir. Android sürümünün çevrimdışı çalışma vaadi Roblox sürümüne aynen taşınmaz.
7. İkon, küçük resimler, TR/EN açıklama, cihaz desteği, hedef kitle ve içerik anketi hazırlanır; yayın erişimi uygun olduğunda açılır.

## Yaş hedefi ve yayın kapıları

Güncel resmî belgede Roblox Kids **5–8**, Select **9–15** yaşları kapsıyor. Bu nedenle Unity sürümünün 4–6 yaş hedefi Roblox'a aynen aktarılmamalı; Roblox denemesi için 5–8 yaşa uygun etkileşimler değerlendirilmelidir. Kids için Minimal veya Mild içerik derecesi gerekir.

16 yaş altına erişim için hesap doğrulaması, iki aşamalı doğrulama, içerik anketi ve yayın değerlendirmesi bulunuyor. Doğrulama yöntemi geliştiricinin yaşına göre değişiyor. Standart değerlendirmede önce yaş kontrolünden geçmiş 16+ oyunculara açılış, son 60 günde Roblox'un nitelikli saydığı kullanıcılardan **250 benzersiz oynanış** ve güvenlik incelemesi var. Bu, 250 eşzamanlı oyuncu şartı değildir. [Roblox Kids ve Select gereklilikleri](https://create.roblox.com/docs/production/publishing/kids-and-select)

Tüm yaşlara yayın/güncelleme için iki aylık aktif Plus/Premium aboneliği veya deneyim başına **1.000 Robux** yayın ücreti seçeneği belirtiliyor. İade koşulludur; ücret ödeme çocuk kitlesine erişim değerlendirmesini atlatmaz. Güncel hesap ekranı ve şartlar karar anında yeniden kontrol edilmelidir. Henüz abonelik veya ödeme yapılmadı. Yeni deneyimler başlangıçta özel erişimlidir. [Oluşturma ve yayın kuralları](https://create.roblox.com/docs/production/publishing/publish-games-and-places)

Türkiye'den erişimin güncel durumu ayrıca gerçek bağlantı ve güncel yetkili duyuruyla doğrulanmalı. Roblox'un ulaşılan resmî Türkiye açıklaması 8 Ağustos 2024 tarihli; bu eski açıklamadan bugünkü erişimin açık veya kapalı olduğu sonucunu çıkarmıyorum. [Tarihli resmî açıklama](https://about.roblox.com/tr/newsroom/2024/08/update-on-the-roblox-ban-in-turkiye)

## Şu an gereken karar

Mevcut Android genişlemesini test ederek ilerletmek ve Roblox'u ayrı uyarlama olarak ele almak. Roblox geliştirmesine geçilirse ilk somut teslim, özel erişimli küçük oynanabilir örnek olmalı. Hesap erişimi, doğrulama, varsa ücret ve değerlendirme tamamlanmadan herkese açık çocuk yayını için tarih verilemez. Google Play hesabı Roblox yayınının bir parçası değildir; Roblox da Google Play'deki bağımsız APK yayınının yerine geçmez.
