# 4–6 yaş için çocuk mobil oyunu — pazar araştırması ve konsept kararı

Araştırma tarihi: **1 Kasım 2026**. İlk aşama teslimi: pazar incelemesi, rakipler, 10 özgün konsept, ağırlıklı değerlendirme ve onay önerisi.

Bu belge nihai GDD değildir. Konseptler ve görsel yön önerileri onaya sunulmaktadır; kesin tasarım, asset üretimi ve oyun kodlaması başlamadı. Yerel teknik erişim durumu [Unity erişim raporunda](UNITY_ERISIM_RAPORU.md) kayıtlıdır.

## 1. Karar özeti

Birinci önerim, **Minik Dostlar – Keşif Adası fikrini küçülterek “Minik Dostlar – Yağmur Bahçesi” çalışma adıyla sınamak**. Çocuk küçük bir bahçede hava, bitki, nesne ve hayvanlar arasında ilişkiler keşfeder. Bir etkileşim diğerinin sonucunu değiştirir; oyuncunun düzenlediği alan sonraki açılışta korunur. Örneğin yağmur bir su birikintisi oluşturur; yaprak burada yüzebilir; hayvan yaprağa farklı tepki verir. Bu, henüz kabul edilmiş oyun tasarımı değil, sınanacak çekirdek fikirdir.

Diğer iki güçlü aday **Bulut Ustası** ve **Tamirci Pati**. Üretim süresi en sıkı kısıtsa Tamirci Pati daha güvenli seçenek; görsel ve sistemsel keşif öncelikliyse Bulut Ustası güçlü alternatif.

Ticari tez: küçük ama özenli bir oyuncak dünya; anlaşılır tek ödeme; reklamsız kullanım; ilk indirmeden sonra temel oyunun çevrimdışı çalışması; okuma gerektirmeyen oyun ve Türkçe/İngilizce ebeveyn desteği. **Bu özelliklerin her biri rakiplerde bulunabiliyor.** Fırsat, bunları düşük içerik hacmiyle de tatmin edici, kendine özgü bir oyun halinde birleştirmekte. Bunun ödeme isteğine dönüşeceği henüz doğrulanmadı.

## 2. Yöntem ve kanıt sınırları

- **Doğrulanmış kaynak bilgisi:** erişilen resmî rapor özeti, geliştirici açıklaması, mağaza sayfası veya görünür yorum. Geliştiricinin eğitim ve güvenlik beyanı bağımsız denetim sonucu değildir.
- **Tasarım çıkarımı:** kaynaklardan hareketle benim değerlendirmem; özellikle “çocuk neden sever”, eksiklik ve ürün önerileri bu gruptadır.
- **Tahmin:** puanlar, üretim süreleri, fiyat denemeleri, maliyetler ve satış senaryoları. Bunlar ölçülmüş pazar sonuçları değildir.

2026 yılı tamamlanmadı. “2026 raporu” çoğunlukla 2025 dönemini anlatıyor; veri yılı ile yayın yılını ayırıyorum. Erişilen mağaza sayfaları ülke, sürüm ve önbelleğe göre değişebilir. ABD fiyatları Türkiye fiyatı değildir. Bir IAP ürününün listelenmesi, her kullanıcıya o anda aynı teklifi gösterdiğini kanıtlamaz.

Rakipleri bu oturumda indirip oynamadım. Aşağıdaki **ilk beş dakika** betimleri mağaza açıklamalarından çıkarılmış olası etkileşim akışlarıdır; abonelik ekranı, öğretici sırası veya gerçek açılış ölçümü değildir. “4+” mağaza içerik derecelendirmesi ile “4–6 yaşa gelişimsel uygunluk” aynı şey değildir.

Sensor Tower'ın herkese açık özetleri kullanıldı; ücretli ülke/yaş bazlı panellere erişilmedi. AppMagic, data.ai, Newzoo ve Statista üzerinden bu nişe ait doğrulanabilir satış serisi alınmadı. Unity sektör raporlarından 4–6 yaş için gelir sonucu çıkarılmadı. Türkiye'de bu dar segmentin büyüklüğü, ebeveyn başına harcama ve rakiplerin net kârı bilinmiyor. İndirme sayısı satın alma veya aktif çocuk sayısı olarak yorumlanmadı.

## 3. 2024–2026 bulguları ve anlamları

| Kanıt | Kaynağın dönemi | Bu proje açısından yorum ve sınır |
|---|---|---|
| Sensor Tower 2025 özeti, 2024 mobil oyun IAP gelirinde %4 artış ve yeni indirmelerde %7 düşüş bildiriyor. | Veri 2024; yayın 11 Mart 2025 | Pazar büyüklüğü tek başına yeni oyunun bulunabilirliğini garanti etmiyor. Genel oyun pazarı; çocuk premium segmenti değil. [Kaynak](https://sensortower.com/press/press-release-sensor-tower-mobile-gaming-rebounds-in-2024-as-player-engagement-and-spending-reach-new-highs) |
| Sensor Tower 2026 özeti mobil oyun gelirini yaklaşık 82 milyar USD ve yıllık artışı %1 olarak veriyor. | Veri 2025; yayın Şubat 2026 | Olgun pazarda farklılaşma gerekli. Önceki raporun yuvarlanmış toplamıyla kendim büyüme hesabı yapmıyorum; reklam ve tüm dağıtım kanallarını kapsayan çocuk pazarı toplamı olarak kullanmıyorum. [Kaynak](https://sensortower.com/press/press-release-sensor-tower-state-of-gaming-gaming-drove-52-billion-downloads-82b-iap-revenue-on-mobile-and-12b-premium-revenue-on-steam) |
| Common Sense'in 2025 açıklaması, ABD'de 0–8 yaşın oyun süresinin önceki dört yılda %65 arttığını bildiriyor. | 2025 yayınlı araştırma | Oyun önemli bir kullanım alanı; daha uzun oynatmayı başarı hedefi yapmamalıyız. ABD 0–8 sonucu Türkiye 4–6 satış talebi değildir. [Kaynak](https://about.commonsensemedia.org/research/the-2025-common-sense-census-media-use-by-kids-zero-to-eight) |
| Qustodio, 2025 kullanımını 4–18 yaş çocukları olan 400 binden fazla aile üzerinden inceliyor; Roblox ABD'de en çok engellenen mobil oyun uygulaması. | Veri 2025; yayın 10 Şubat 2026 | Güvenlik kaygısı görünür; ebeveyn kontrolü kullanan seçilmiş örneklem ve geniş yaş aralığı nedeniyle bütün ailelere veya 4–6 yaşa genellenemez. [Kaynak](https://www.qustodio.com/en/research/annual-report-2025/) |
| Pok Pok Android'e 10 Ekim 2024'te geldi. | Ürün olayı 2024 | “Sakin kaliteli keşif Android'de yok” iddiası geçersiz. [Geliştirici duyurusu](https://playpokpok.com/blog/pok-pok-is-now-on-the-google-play-store/) |

Sonuç bir gelir tahmini değildir: yaratıcı oyun ve ebeveyn güveni etrafında güçlü rekabet var. Küçük stüdyo için yüzlerce etkinlik üretmek yerine birkaç etkileşimi daha anlamlı, anlaşılır ve tekrar kullanılabilir yapmak savunulabilir bir ürün stratejisidir.

## 4. Rakip analizi

Aşağıdaki öğrenme katkıları, etkinliklerin sunduğu alıştırma fırsatlarıdır; oyunun bilimsel olarak gelişimi artırdığının kanıtı değildir. Şikâyet kanıtı olmayan rakibe şikâyet uydurulmadı. Her kartın sonundaki “bizim çıkarımımız” satırı gözlemden ayrıdır.

### Toca Boca World — açık uçlu hikâye ve dekorasyon

**Doğrulanan:** karakter ve oda özelleştirme, rol yapma, ücretsiz başlangıç ve içerik satın alımları; mağazada 6–8 yaş için tasarlandığı belirtiliyor. 4+ işareti 4 yaşta bağımsız oynanabilirlik kanıtı değil. [App Store](https://apps.apple.com/us/app/toca-boca-world-game-play/id1208138685)

**Akış çıkarımı:** ilk dakikalarda bir yere girme, karakteri/nesneyi taşıma ve küçük hikâye kurma. Döngü düzenle → canlandır → başka senaryo kur. Yeniden dönüşü oyuncunun hikâyeleri ve yeni içerikler destekler. Tanınabilir siluetler, kıyafetler ve gündelik nesneler rol yapmayı kolaylaştırır; dil ve hayal gücü için fırsat sağlar. Ebeveyn açısından yaratıcı serbestlik değerli olabilir. Görünür bir yorum hata sorunundan söz ediyor; bütün sürümlerin durumunu göstermez.

**Bizim çıkarımımız:** sahiplik hissini al; yüzlerce oda, haftalık hediye döngüsü, marka işbirlikleri ve paket mağazasını taklit etme. Küçük yaşta yoğun arayüz ve içerik ölçeği bizim için uygun referans değil.

### Sago Mini World — çok sayıda oyuncak etkinliği

**Doğrulanan:** geliştirici açıklaması 2–6 yaşı hedefliyor; rol yapma ve farklı temalı etkinlikler sunuyor. Abonelik/Piknik planları var; indirilen oyunların çevrimdışı kullanımından söz ediliyor. [Mağaza](https://apps.apple.com/gb/app/sago-mini-world-kids-games/id874425722)

**Akış çıkarımı:** etkinlik seç → dokun/sürükle → karakterin komik tepkisini izle. Tekrar, başka nesne ve rollerden doğar. Renkli sade şekiller ve okunabilir ifadeler hayvanları oyuncak gibi erişilebilir kılar. Hayal gücü ve günlük yaşam canlandırması öne çıkar. Yorumlarda sevimlilik/animasyon övgüsü ve abonelikle erişim şikâyeti görülüyor; bazı yorumcular çocuk, hepsi ebeveyn değil. [Görünür yorumlar](https://apps.apple.com/us/app/sago-mini-world-kids-games/id874425722?platform=iphone&see-all=reviews)

**Bizim çıkarımımız:** tepkilerin kalitesini örnek al; içerik koleksiyonuyla hacim yarışına girme. Geliştirme riski, her yeni oyuncağın ayrı tasarım ve test gerektirmesi.

### Pok Pok — sakin dijital oyuncaklar

**Doğrulanan:** 2–8 yaşa yönelik, açık uçlu ve kazanma/kaybetme baskısı olmayan etkinlikler; yumuşak sesler, sade çizimler. Aboneliklerin yanında listede **Lifetime** ürünü de var. [Mağaza](https://apps.apple.com/us/app/pok-pok-montessori-preschool/id1550204730)

**Akış çıkarımı:** oyuncak seç → dene → sonuçları birleştir. İlk dakikalarda kural çözmek yerine keşif; tekrar oynama aynı araçları farklı kullanmaktan doğar. Grafik biçimler ve ses, karakter sayısından daha baskın olabilir. Neden-sonuç ve yaratıcılık için fırsat sunar. Yorum örneklerinde sakinlik övgüsü; düzenlemeyi saklama, harita kaydırma ve fiyat konuları var. [Yorumlar](https://apps.apple.com/us/app/pok-pok-montessori-preschool/id1550204730?platform=iphone&see-all=reviews)

**Bizim çıkarımımız:** en güçlü doğrudan karşılaştırmalardan biri. Sakinlik veya tek ödeme tek başına farkımız olamaz. Daha belirgin hayvan ilişkileri ve kalıcı küçük bahçe hipotezini test et; çizim dilini, oyuncaklarını ve arayüzünü kopyalama.

### Bimi Boo — kısa öğrenme etkinlikleri

**Doğrulanan:** bir oyun değil, ürün ailesi. Örneklenen “Kids Learning Games & Stories” 2–5 yaş açıklamasıyla çizgi öykü ve eşleştirme/bulmaca etkinliklerini birleştiriyor; abonelik ürünleri listeleniyor. [Mağaza](https://apps.apple.com/us/app/kids-learning-games-stories/id1537964112)

**Akış çıkarımı:** bölüm/aktivite seç → eşleştir veya sırala → geri bildirim al. Tekrar, yeni etkinlik ve aynı beceriyi yeniden denemeden gelir. Büyük şekiller, belirgin renkler ve tanınabilir maskot arayüzü kolaylaştırabilir. Öğrenme hedefinin açık oluşu ebeveyn değer önerisidir; etkinliklerin gelişim etkisi bağımsız doğrulanmış sayılmaz. Bu ürün için yeterli yorum örneği incelenmedi.

**Bizim çıkarımımız:** kısa ve anlaşılır eylemleri al. Oyunu “doğru kutuya sürükle” alıştırmalarına indirgeme; çok sayıda etkinlik geliştirme yüküne ve 6 yaşta fazla basit kalma riskine dikkat et. Rakip maskotunu veya bulmaca düzenlerini kopyalama.

### LEGO DUPLO World — yapı kurma ve temalı oyun

**Doğrulanan:** 3–6 yaş, 3D bloklarla kurma, temalı etkinlikler ve birlikte oynamaya yönelik çoklu dokunma; mağazada abonelik ve ayrı içerik ürünleri birlikte listeleniyor. [Mağaza](https://apps.apple.com/us/app/lego-duplo-world/id1458749093)

**Akış çıkarımı:** temaya gir → blok/nesne yerleştir → araç veya hayvanla canlandır. Tekrar farklı yapılarla sağlanır. Tanıdık blok formu ve oyuncak hissi görsel gücüdür; marka güveni yeni stüdyonun hazır sahip olduğu avantaj değildir. Mekânsal düşünme ve canlandırma fırsatı sunar. Yorum örneklemi kurulmadığından ebeveyn şikâyeti hakkında sonuç yok.

**Bizim çıkarımımız:** serbest kurma ve kolay bağlantı faydalı. Lisanslı blok görünümünü ve karakterleri kopyalama. Çok sayıda serbest parçanın çakışma, görünürlük ve kurtarma sorunları üretim maliyetini artırır.

### Thinkrolls — mantık ve fizik bulmacaları

**Doğrulanan:** geliştirici abonelik, tek seferlik School Edition/Lifetime ve ayrı Pay-Once uygulamalar sunuyor; amiral ürün 2–8 yaş hedefliyor. [Geliştirici](https://www.avokiddo.com/apps/)

**Akış çıkarımı:** yolu incele → engeli hareket ettir → karakteri ilerlet → yeniden dene. İlk dakikalarda tek bir neden-sonuç kuralı tanınabilir; dönüşü yeni çözümler sağlar. Yuvarlak, okunabilir karakterler işleve uyumlu. Planlama için alıştırma sunar; ebeveyn için açık hedef avantaj, çözümde takılma ise olası risk. Güncel yorum örneklemi yok.

**Bizim çıkarımımız:** sade kuralların çeşitlenmesini öğren; yüzlerce bölümü veya aynı fizik araçlarını kopyalama. Ayrı uygulama ile pahalı kapsamlı paketin fiyatını karıştırma. Yaşa göre güçlük ayarı ve ipucu testi bizim bulmaca adaylarımız için zorunlu olmalı.

### Little Kitten — tek karakterle duygusal bağ

**Doğrulanan:** 2+ yaş açıklaması, kediyle saklambaç, boya ve odada keşif; gerçekçi sevimli animasyonlar ve az menü. Güncel Birleşik Krallık sayfası ücretsiz başlangıç ve Unlimited planları gösteriyor; eski “sadece ücretli uygulama” kabulü kullanılmadı. [Mağaza](https://apps.apple.com/gb/app/little-kitten-cat-games/id1171095810)

**Akış çıkarımı:** kediyi keşfet → nesneye dokun → komik tepki → başka etkinlik. Tekrarın merkezinde karaktere yakınlık bulunur. Neden-sonuç ve bakım canlandırması fırsatları var. Ebeveyn memnuniyeti veya şikâyeti için yeterli güncel yorum okunmadı.

**Bizim çıkarımımız:** bir maskotun ifade kalitesi dört zayıf karakterden değerli olabilir. Gerçekçi kürk/animasyon kalitesini düşük bütçeyle hedeflemek pahalıdır; kediyi veya odasını kopyalamadan sade, özgün siluete yatırım yap.

### PBS KIDS Games — ücretsiz içerik alternatifi

**Doğrulanan:** tanınan PBS karakterleriyle ücretsiz eğitsel etkinlikler; Google Play sayfasında çevrimdışı etiketi bulunuyor. 31 Mart 2026 yorumunda indirme/çevrimdışı kullanım kaybı şikâyeti, 2 Nisan geliştirici yanıtında işlevi ekleme niyeti görülüyor. Eski yorum bugünkü sürümün çalışmadığını kanıtlamaz. [Google Play](https://play.google.com/store/apps/details?id=org.pbskids.gamesapp)

**Akış çıkarımı:** karakter veya etkinlik seç → kısa oyunu tamamla → başkasını seç. Tekrar içerik çeşitliliği ve bilinen karakterlerden gelir. Öğrenme alanı etkinliğe bağlıdır; bütün koleksiyona tek sonuç atanamaz. Görsel dil marka evrenleri arasında değişir.

**Bizim çıkarımımız:** ücretsiz güçlü alternatif bulunduğu için yalnız “eğitici oyun” satmak zor. Kesintisiz çevrimdışı kullanımın seyahatteki değeri test edilmeli; içerik hacmini veya tanınmış karakterleri taklit etme.

### Roblox — dolaylı özgürlük referansı

**Doğrulanan:** tek oyun değil, çok sayıda deneyim içeren platform. 2026 resmî açıklamalarında Roblox Kids 5–8 ve Select 9–15 hesapları, yaş kontrolleri ve ebeveyn ayarları bulunuyor. Dolayısıyla eski güvenlik özellikleri üzerinden değerlendirme yapılmamalı. [Roblox güvenlik açıklaması](https://about.roblox.com/safety-faqs)

**Akış çıkarımı:** deneyim seç → avatarla etkileş → başka deneyim/arkadaş etkinliğine geç; ilk beş dakika deneyime göre değişir. Yenilik, sosyal bağ ve avatar kimliği dönüşü destekleyebilir. Görsel/öğrenme niteliği içerikten içeriğe değişir. Ebeveyn kaygısı için yukarıdaki Qustodio verisi bir sinyaldir, bütün ürünlere hüküm değildir.

**Bizim çıkarımımız:** seçim hakkını al; platform altyapısı, kullanıcı içerikleri, sohbet, sanal para ve sonsuz içerik akışını kapsam dışında tut. 4 yaş için doğrudan uygunluk veya ücretli tek oyunculu oyuna satış emsali sayma.

### Minecraft — dolaylı inşa referansı

**Doğrulanan:** bloklarla üretme ve keşif; çevrimiçi kullanım ve çocuk hesapları için resmî ebeveyn rehberleri var. [Minecraft rehberi](https://www.minecraft.net/en-us/article/parents--guide-minecraft)

**Akış çıkarımı:** dünyaya gir → nesne/blok seç → yerleştir veya keşfet. Serbest üretim tekrar sağlar; blok estetiği kolay tanınır. Mekânsal kurma ve planlama fırsatları var. Buna karşılık kamera, araç seçimi ve arayüz, 4 yaşta bağımsız kullanımı ayrıca sınamayı gerektirir. Bu oturumda güncel bölgesel fiyat ve yorum örneklemi alınmadı; satın alma/ek hizmet yapısı sürüme göre doğrulanmalı.

**Bizim çıkarımımız:** dünyanın değiştirilebilirliğini örnek al. Hayatta kalma, büyük dünya, karmaşık envanter, sunucu ve blok kimliğini kopyalama. Bizim yaş grubuna doğrudan satış kanıtı değildir.

### Bluey: The Videogame — ailece canlandırma referansı

**Doğrulanan:** PC/konsol ürünü; ailece yerel birlikte oyun, öykü ve diziden esinlenen etkinlikler. Bu mobil oyunla veya başka Bluey uygulamalarıyla karıştırılmamalı. [Yayıncı](https://outrightgames.com/games/bluey-the-videogame)

**Akış çıkarımı:** aile karakteriyle dolaş → basit etkinliğe katıl → öykü/oyun değiştir. Dönüşü birlikte oyun ve karakter tanışıklığı destekleyebilir. Tanıdık dünya güçlü görsel ve duygusal sermaye sağlar; sosyal canlandırma fırsatı vardır. Güncel fiyat ve ebeveyn yorumları doğrulanmadı.

**Bizim çıkarımımız:** gündelik durumların komik oyuna dönüşmesini öğren. Lisanslı karakterleri, dizinin görünümünü, kontrolcü gerektiren etkileşimleri veya aynı marka talebini varsaymayı kopyalama. Tek oyunculu mobil kapsamımız için dolaylı referanstır.

### Animal Crossing: Pocket Camp Complete — ücretli mobil dekorasyon referansı

**Doğrulanan:** iOS/Android için 19,99 USD; uygulama içi eşya satın alımı yok. Sürekli bağlantı olmadan oynanabilme belirtiliyor. [Resmî ürün sayfası](https://ac-pocketcamp.com/en-US) · [Destek açıklaması](https://faq.complete.ac-pocketcamp.com/hc/en-us/articles/38440141011993-About-Pocket-Camp-Complete)

**Akış çıkarımı:** hayvanlarla tanış → eşya/dekorasyonla kampı düzenle → başka kombinasyon dene. Tekrarın kaynağı kişisel mekân, koleksiyon ve karakter bağları. Sevimli oranlar ve tutarlı çevre sanatı güçlüdür. 4–6 yaşa okumasız bağımsız uygunluk bu kaynaklardan doğrulanmıyor; menü/ekonomi yükü risk. Güncel ebeveyn yorumları örneklenmedi.

**Bizim çıkarımımız:** kalıcı kişisel alanı ve ücretli ürün örneğini öğren. Bilinen markanın yıllarca biriken içerik değerini küçük yeni oyunun fiyatına emsal yapma; zaman baskısını, koleksiyon ölçeğini veya karakterlerini kopyalama.

### Pokémon Playhouse — yaşa yakın tasarım referansı

**Doğrulanan:** resmî sayfa 3–5 yaş, okuma ve matematik gerektirmeyen etkinlikler tanımlıyor. Sayfanın varlığı 2026'da her bölgede indirme ve desteğin sürdüğü anlamına gelmiyor; güncel mağaza erişimi ve fiyatı bu çalışmada doğrulanmadı. [Pokémon](https://www.pokemon.com/uk/app/pokemon-playhouse)

**Akış çıkarımı:** karakteri/etkinliği seç → basit dokunuşlarla ilgilen → başka karakter tepkisini keşfet. Tanınmış canlılarla bağ kurmak çekicilik ve tekrar sağlayabilir; oyun içindeki hayvan benzeri davranışlar gerçek doğa eğitimi sayılmaz. Ebeveyn yorum kanıtı yok.

**Bizim çıkarımımız:** okuma gerektirmeyen bakım ve keşif iyi tasarım referansıdır. Karakter toplama ekonomisini, yakalama mekaniklerini veya Pokémon görsel dilini kopyalama. Güncel ticari rakip başarısı olarak kullanma.

## 5. Gerçek yorumlardan sınırlı nitel örneklem

Bu tablo sekiz görünür yorumdan seçilmiş temaları içerir; sistematik örneklem, anket veya kullanıcıların çoğunluğunu temsil eden oran değildir. Yinelenen gösterimler ayrı yorum sayılmadı. Yorum yazarının ebeveynliği ancak kendi anlatımına dayanır. Tam metin alıntılanmadı.

| Ürün / yorum tanımı | Olumlu veya olumsuz sinyal | Tasarımda sınanacak karşılığı |
|---|---|---|
| Pok Pok, WebbK, 6 Mayıs 2024; dört çocuğundan söz ediyor | Farklı yaşların farklı kullanım biçimleri, sakin keşif ve görsel övgü | Tek nesneye birden çok oyun imkânı. [Kaynak](https://apps.apple.com/us/app/pok-pok-montessori-preschool/id1550204730) |
| Pok Pok, “Love It”, 18 Haziran; yıl görünmüyor | Dünyayı oluşturduktan sonra saklama isteği | Yerel kayıt ve geri yükleme. [Kaynak](https://apps.apple.com/us/app/pok-pok-montessori-preschool/id1550204730?platform=iphone&see-all=reviews) |
| Pok Pok, “Perfect for travel”, 27 Ağustos; yıl görünmüyor | Seyahat kullanımı olumlu; haritanın fazla kayması zorlayıcı | Sabit kamera veya kontrollü sayfalama testi. [Kaynak](https://apps.apple.com/us/app/pok-pok-montessori-preschool/id1550204730?platform=iphone&see-all=reviews) |
| Pok Pok, “5 stars so only you can see”, 10 Mayıs; yıl görünmüyor | Ücret beklentisi uyuşmazlığı | Fiyatı mağazada ve ebeveyn alanında açık anlatmak. [Kaynak](https://apps.apple.com/us/app/pok-pok-montessori-preschool/id1550204730?platform=iphone&see-all=reviews) |
| Sago Mini, “Bad game”, 20 Ağustos; yıl görünmüyor; kızından söz ediyor | Abonelik nedeniyle beklenen içeriğe erişememe | Ücretsiz/ücretli kapsam varsa yetişkine açık anlatım. [Kaynak](https://apps.apple.com/us/app/sago-mini-world-kids-games/id874425722?platform=iphone&see-all=reviews) |
| Sago Mini, “so entertaining!”, 20 Haziran; yıl görünmüyor | Karakter, ifade ve animasyon övgüsü | Az karakterde daha iyi tepki kalitesi. [Kaynak](https://apps.apple.com/us/app/sago-mini-world-kids-games/id874425722?platform=iphone&see-all=reviews) |
| Toca Boca World, “The bug is getting worse”, 21 Haziran; yıl görünmüyor | Oyunu sevmesine rağmen hata şikâyeti | Kayıt, uzun oturum ve güncelleme testleri. [Kaynak](https://apps.apple.com/us/app/toca-boca-world-game-play/id1208138685) |
| PBS KIDS, Jana Grace Fox, 31 Mart 2026 | Seyahatte çevrimdışı kullanımın kaybı; bunun için ödeme isteği belirtiyor | Tek kişinin beyanı pazar fiyat araştırması değildir; uçak modunda test gereğini destekler. [Kaynak](https://play.google.com/store/apps/details?id=org.pbskids.gamesapp) |

Buradan çıkarılabilecek sonuç, bu sorunların **var olabildiğidir**. “Ebeveynlerin çoğu abonelik istemiyor” veya “kayıt özelliği satışları artırır” sonuçlarını bu örneklemle kuramayız. Ücretsiz alternatifler bulunduğu için reklam olmaması da tek başına ödeme gerekçesi sayılmaz.

## 6. 4–6 yaş için tasarım ilkelerinin değerlendirilmesi

CDC'nin dört yaş gelişim örnekleri rol yapma, yardım etme, bazı renkleri adlandırma ve basit motor becerileri içeriyor. Bunlar her çocuk için zorunlu performans şartı veya oyunla öğretilebileceğinin kanıtı değildir. [CDC: 4 yaş](https://www.cdc.gov/act-early/milestones/4-years.html)

Beş yaş için verilen 5–10 dakikalık dikkat örneği ekran dışı etkinlikler içindir; kaynak özellikle ekran süresini hariç tutuyor. Bu sayıyı “oyunun ideal süresi” diye kullanmıyorum. [CDC: 5 yaş](https://www.cdc.gov/act-early/milestones/5-years.html)

| Başlangıç ilkesi | Değerlendirme | Ürün kararı / test hipotezi |
|---|---|---|
| Basit dokunma ve sürükleme | Güçlü başlangıç hipotezi; sürükleme yine hata yaratabilir | Büyük hedef, geniş bırakma alanı; gerektiğinde dokun-seç/dokun-bırak alternatifi |
| İlk saniyelerde anlaşılma | Ticari ve kullanılabilirlik hedefi; bilimsel eşik değil | İlk anlamlı tepkiyi 30 saniye içinde keşfetme testi |
| Sevimli karakter | Rakiplerin yaklaşımı; evrensel tercih değil | İki görsel yönü çocuk ve ebeveynle karşılaştırma |
| Kısa seans | Tamamlama ve bırakma kolaylığı önemli | 30–90 saniyelik küçük eylemler; seansı uzatan ödül yok; süreler tasarım tahmini |
| Açık uçlu özgürlük | Yaratıcılığa alan açar; bazı çocuklar amaçsız kalabilir | İsteğe bağlı görsel öneri; zorunlu görev dizisi yok |
| Sakinlik | Düşük ses/uyaran olumlu sinyal; tepkisizlik demek değil | Dokunuşa hemen hareket/ses, üst üste efektleri sınırlama |
| Doğal öğrenme | Neden-sonuç ve sınıflama için uygun fırsat | Harf okuma, çok adımlı talimat ve süreli doğru cevap zorunluluğu yok |
| Tekrar oynama | İçerik sayısıyla eşit değil | Aynı nesneleri farklı biçimde kullanabilme; günlük giriş baskısı yok |
| Ebeveyn güveni | Ürün tasarımının ve dağıtımın parçası | Hesap, sohbet, reklam, bildirim ve kişisel veri talebini çekirdekten çıkarma |
| Düşük karmaşıklık | Küçük bütçeyle uygulanabilir | Tek kamera, az etkileşim kuralı; görsel kaliteyi içerik sayısına feda etmeme |

Renk tek başına görev ayracı olmamalı; şekil ve simge de kullanılmalı. Ses kapalıyken temel oyun anlaşılmalı. Türkçe ve İngilizce kısa sesli ipuçları ileride denenebilir; ses kaydı izni gerektiren özellik düşünülmüyor. Duygu etkileşimlerinde tek “doğru yüz = doğru duygu” kalıbından kaçınmak gerekir.

## 7. Pazar fırsatı: kanıt ve henüz sınanmamış boşluk

**Gözlenen rekabet:** açık uçlu oyun, kısa eğitim etkinliği, inşa, mantık bulmacası ve karakter bakımı için ürünler mevcut. Ürün varlığı ve değerlendirme hacmi talep sinyalidir; tek başına kârlılık veya 4–6 yaşın payını göstermez.

**Sınanacak teklif:** “Çocuğun kendi küçük bahçesini değiştirip bıraktığı gibi bulduğu, hayvanlarla bağlantılı keşif oyunu; tek ödeme, reklamsız ve temel içerikte çevrimdışı.” Burada yenilik iddiası bahçe temasına değil, az sayıda araçtan çıkan birbirine bağlı sonuçların kalitesine dayanmalı.

Karşılanmamış olduğu henüz kanıtlanmayan ihtiyaçlar: basit kamerayla küçük yaşta bağımsız oyun, güvenilir kayıt, küçük indirme boyutu, Türkçe yetişkin açıklamaları ve açık fiyat. Sago/Pok Pok incelenen dil listelerinde Türkçe görünmüyor; Toca Boca ve Little Kitten'da görünüyor. Bu yüzden “Türkçe çocuk oyunu yok” denemez. Dil az kullanan oyunda yerelleştirmenin ticari etkisi ayrıca sınanmalı. [Sago](https://apps.apple.com/us/app/sago-mini-world-kids-games/id874425722) · [Pok Pok](https://apps.apple.com/us/app/pok-pok-montessori-preschool/id1550204730) · [Toca Boca](https://apps.apple.com/us/app/toca-boca-world-game-play/id1208138685) · [Little Kitten](https://apps.apple.com/gb/app/little-kitten-cat-games/id1171095810)

Küçük geliştirici için avantaj üretilebilecek alanlar: tek güçlü maskot, tutarlı görsel dünya, sağlam kayıt/geri dönüş, okunaksız menülerin azaltılması, makul paket boyutu ve belli bir oyunun çok iyi yapılması. Yeni bahçe/araç paketleriyle genişleme mümkün; ilk satış vaadi, gelecekte gelecek içeriğe bağımlı olmamalı.

## 8. On özgün oyun konsepti

Tüm adlar **geçici çalışma adlarıdır**; marka müsaitliği araştırılmış değildir. Hepsi 4–6 yaş, Android/iOS, başlangıçta Türkçe/İngilizce ve reklamsız tek ödeme yaklaşımıyla düşünülmüştür. Süreler tek geliştiricinin haftada yaklaşık 25–30 odaklı saat çalıştığı, kullanılabilir sanat/ses üretim desteği olduğu varsayımıyla **ilk satılabilir küçük sürüm için kişisel tahminlerdir**. Öğrenme süresi, mağaza incelemesi, cihaz temini ve büyük sanat revizyonları ayrıca uzatabilir. Fikirler aynı eşleştirme oyununun tema varyasyonları değildir.

### 1. Minik Dostlar – Yağmur Bahçesi

- **Hikâye / oyuncu:** dört küçük hayvan ortak bahçelerini keşfeder; özellikle canlandırma ve hayvanları seven 4–6 yaş.
- **Ana karakter / dünya:** yaprak biçimli kulak ayrıntısı olan meraklı bir tavşan adayı; tek ekranda okunabilir bahçe, küçük gölet ve piknik köşesi.
- **Temel oynanış / aktiviteler:** nesneyi kullan → dünyadaki değişimi gör → yeni ilişki dene. Bulutu hareket ettirme, filiz sulama, yaprak yüzdürme, hayvana nesne götürme ve alan düzenleme.
- **Eğlence / öğretici unsur:** beklenmedik ama anlaşılır hayvan tepkileri; neden-sonuç, büyük/küçük, sıra ve yardım etme canlandırması. Açlık cezası veya günlük bakım zorunluluğu yok.
- **Görsel yön:** yumuşak 2D hayvanlar, sabit açılı katmanlı 2.5D bahçe; büyük okunabilir siluetler.
- **Fark / tekrar:** nesneler tek animasyon düğmesi olmaktan çıkar; aynı yaprak hava ve su durumuna göre farklı oynanır. Saklanan düzen yeni hikâyeye zemin olur. Rakiplerde benzer unsurlar var; üstünlük test gerektirir.
- **Zorluk / kapsam:** orta-yüksek; bir alan, dört hayvan, üç ana etkinlik ailesi, 8–10 dekor, yaklaşık 20 kısa tepki; **18–26 hafta**.
- **Ticari potansiyel / risk:** maskot ve tek bakışta anlaşılır sahne güçlü satış anlatısı sağlayabilir. En büyük risk birleşen etkileşimlerin test yükü ve küçük alanın çabuk tüketilmesi. Bu yüzden önce tek hayvan/üç etkileşimlik örnek gerekir.

### 2. Yaprak Postası

- **Hikâye / oyuncu:** ormandaki komşulara resimli paketler ulaştırılır; rota keşfinden hoşlanan 5–6 yaş, basitleştirilmiş 4 yaş modu.
- **Ana karakter / dünya:** sakin salyangoz postacı; üç kısa dallanmış patikadan oluşan orman maketi.
- **Temel oynanış / aktiviteler:** adres resmi seç → yol parçası yerleştir → postacıyı izlet. Köprü kurma, farklı geçişleri deneme, paketi resimle sahibine götürme.
- **Eğlence / öğretici unsur:** paketin açılmasıyla komik küçük olaylar; sıra kurma, görsel bellek ve yön ilişkileri. Adres okumak gerekmez.
- **Görsel yön:** kâğıt maket yollar, iri ev sembolleri ve yavaş, belirgin karakter hareketi.
- **Fark / tekrar:** hız veya teslimat süresi yerine oyuncunun kurduğu rota önemlidir; aynı alıcıya birkaç yol mümkün.
- **Zorluk / kapsam:** orta; üç maket, altı alıcı, 18–24 rota düzeni, 12 paket; **14–20 hafta**.
- **Ticari potansiyel / risk:** ebeveyne anlatılabilir bir problem çözme oyunu. Bölüm üretimi maliyetli; görevlerin tekrara düşmesi ve dört yaş için yol planlamanın ağırlaşması olası. Yeni bölgelerle genişleyebilir, ancak ilk oyun yeterli rota çeşitliliği taşımalı.

### 3. Bulut Ustası

- **Hikâye / oyuncu:** küçük bir bulut, minyatür doğa sahnelerine hava götürür; deneme ve dönüşüm seven 4–6 yaş.
- **Ana karakter / dünya:** üç farklı yüz ifadesi olan yuvarlak bulut; gölet, çayır ve yel değirmeni maketleri.
- **Temel oynanış / aktiviteler:** yağmur, güneş ve rüzgâr araçlarını sürükle; yaprak uçur, çark döndür, su oluştur ve filiz büyüt.
- **Eğlence / öğretici unsur:** anında görünür dönüşüm, küçük zincirler; neden-sonuç, tahmin ve değişken deneme. Hava olaylarının gerçek bilimsel simülasyonu olduğu iddia edilmez.
- **Görsel yön:** pastel maket dioramaları, kontrollü parçacıklar, büyük hava araçları.
- **Fark / tekrar:** görev tamamlamak yerine aynı ortamın farklı koşullarda nasıl davrandığını keşfetmek; araç sırasını değiştirmek.
- **Zorluk / kapsam:** orta; üç küçük sahne, üç araç, 12 tepkisel nesne ve yaklaşık 15 sonuç kuralı; **14–20 hafta**.
- **Ticari potansiyel / risk:** kısa tanıtım videosunda kolay anlaşılır ve yeni biyomlara genişleyebilir. Hayvan maskotuna göre bağ kurma zayıf kalabilir; koşul kombinasyonları ve efekt optimizasyonu bütçeyi zorlayabilir. Tepkiler tek kullanımlık numaraya dönüşürse tekrar değeri düşer.

### 4. Cep Orkestrası

- **Hikâye / oyuncu:** küçük canlılar kendi konserini kurar; müzik ve tekrar eden hareketlerden hoşlanan 4–6 yaş.
- **Ana karakter / dünya:** yaprak kulaklı bir ritim faresi; masa üstü açık hava sahnesi.
- **Temel oynanış / aktiviteler:** müzisyeni halka üzerine koy → döngüyü dinle → başka ses ekle/çıkar. Tempo seçme, kısa ses dizisi kurma, hayvanların hareketlerini birleştirme.
- **Eğlence / öğretici unsur:** her düzenin farklı müzik üretmesi; ritim, örüntü ve sırayla katkı. Nota okuma ve performans puanı yok.
- **Görsel yön:** az sayıda canlı renk, hareketle anlaşılır vuruşlar, küçük sevimli grup.
- **Fark / tekrar:** doğru şarkıyı çalma yerine beste oyuncaklığı; saklanan düzenlere dönme.
- **Zorluk / kapsam:** orta; altı ses katmanı, sekiz karakter pozu, üç ortam, 12 yerel beste yuvası; **12–18 hafta**.
- **Ticari potansiyel / risk:** içerik parçaları yeniden birleştirilebilir, ebeveynle ortak oyuna uygundur. Ses kalitesi ve gecikme uzmanlık ister; gürültülü karışımlar ebeveyni uzaklaştırabilir. Yeni enstrümanlarla genişleme mümkün; ses kapalıyken eğlence büyük ölçüde azalır.

### 5. Tamirci Pati

- **Hikâye / oyuncu:** mahalledeki oyuncaklar onarılıp komik yeni biçimlere dönüştürülür; somut hedef seven 4–6 yaş.
- **Ana karakter / dünya:** yumuşak kahverengi kunduz tamirci; tek, düzenli masa ve küçük deneme alanı.
- **Temel oynanış / aktiviteler:** incele → parça seç → yerine tak → çalıştır. Tekerlek, kol, kanat ve renk parçalarını farklı kombinasyonlarda kullanma.
- **Eğlence / öğretici unsur:** onarılan oyuncağın hareketini görmek; parça-bütün, şekil, işlev ve deneme. Birden çok çalışan çözüm bulunur; yanlış parça ceza vermez.
- **Görsel yön:** dokulu ahşap masa, iri kenarlı oyuncaklar, belirgin mekanik hareketler.
- **Fark / tekrar:** sabit çözümü olan yapboz yerine işleyen komik varyasyonlar; her modelin birkaç sonucu.
- **Zorluk / kapsam:** orta-düşük; tek masa, bir maskot, 12 temel oyuncak ve yaklaşık 24 parça; **12–18 hafta**.
- **Ticari potansiyel / risk:** küçük kapsamı parlatmak kolay; “tamir et, kendi oyuncağını yap” vaadi anlaşılır. Sonuçlar yeterince farklı değilse sürükle-bırak alıştırmasına dönüşür. Açık dünya hissi zayıftır; yeni oyuncak setleriyle genişleyebilir.

### 6. Gölge Masalları

- **Hikâye / oyuncu:** çocuk kendi sessiz kukla gösterisini kurar; rol yapmayı seven 4–6 yaş.
- **Ana karakter / dünya:** perde arkasındaki sevimli güve sunucu; üç küçük tiyatro dekoru.
- **Temel oynanış / aktiviteler:** kukla ve sahne seç → taşı → hazır hareket/ses seç → kısa gösteriyi oynat. Yalnız oyun içi hareket dizisi yerelde saklanır; mikrofon kaydı yok.
- **Eğlence / öğretici unsur:** karakterlerin beklenmedik karşılaşmaları; olay sıralama, canlandırma ve birlikte öykü anlatma fırsatı.
- **Görsel yön:** sıcak ışık, kesme kâğıt siluetler, sade kukla yüzleri.
- **Fark / tekrar:** cevap verilen öykü yerine çocuğun kurduğu gösteri; aynı kuklalarla farklı olaylar.
- **Zorluk / kapsam:** orta-yüksek; altı kukla, üç dekor, 12 hareket, basit yerel oynatma sistemi; **16–24 hafta**.
- **Ticari potansiyel / risk:** özgün mağaza görüntüsü ve ailece oyun imkânı. Serbest hikâye kurma bazı çocukları yönsüz bırakabilir; hareket kaydı ve düzenleme arayüzü küçük yaş için karmaşıklaşabilir. Yeni öykü malzemeleriyle genişler, ama çok düğmeli editöre dönüşmemeli.

### 7. Yuvarlak Yollar

- **Hikâye / oyuncu:** yuvarlak bir böcek yuvasına ulaşır; görsel bulmaca seven 4–6 yaş.
- **Ana karakter / dünya:** kabuğunda tek yaprak işareti olan tombul böcek; küçük yamaç parkurları.
- **Temel oynanış / aktiviteler:** rampa ve köprü koy → hareketi başlat → sonucu izle → düzenle. Hızlı refleks veya düşme cezası yok; güvenli başlangıca dönüş var.
- **Eğlence / öğretici unsur:** yuvarlanma, zıplama ve kestirme bulma; yön, eğim ve öngörü için deneyim.
- **Görsel yön:** basit, açık renkli parkurlar; arka planla güçlü ayrım.
- **Fark / tekrar:** birden çok güvenli çözüm ve bölüm sonunda serbest deneme. Thinkrolls gibi güçlü rakipler nedeniyle farkın gerçekten görünmesi gerekir.
- **Zorluk / kapsam:** orta; üç mekanik, yaklaşık 30 kısa bölüm, üç görsel tema; **14–20 hafta**.
- **Ticari potansiyel / risk:** satılabilir kapsam net, oynanış kolay gösterilir. Seviye tasarımı ve fizik tutarlılığı maliyeti yüksek; çözülmüş bölümün tekrar değeri sınırlı. Parkur paketleriyle genişler, zamanlayıcı ve yıldız baskısı eklenmez.

### 8. Minik Ev Mimarları

- **Hikâye / oyuncu:** farklı hayvanlara kendilerine uygun evler kurulur; inşa ve dekorasyon seven 4–6 yaş.
- **Ana karakter / dünya:** yavaş, düşünceli kaplumbağa mimar; üç küçük inşaat adacığı.
- **Temel oynanış / aktiviteler:** oda, çatı ve rampa seç → otomatik bağlantıyla yerleştir → hayvanın kullanışını izle. Odaları renklendirme ve evi yeniden kurma.
- **Eğlence / öğretici unsur:** absürt ama kullanılabilir evler; mekânsal ilişki, büyüklük ve planlama. Para biriktirme veya inşaat bekleme süresi yok.
- **Görsel yön:** oyuncak ahşap yapılar ve yandan/üstten sabit açı.
- **Fark / tekrar:** yapıyı bitirince hayvanın farklı kullanım animasyonları; kayıtlı evleri değiştirme.
- **Zorluk / kapsam:** yüksek; üç hayvan, yaklaşık 20 modül, bağlantı kuralları ve güvenilir serbest yerleştirme; **20–30 hafta**.
- **Ticari potansiyel / risk:** güçlü kişiselleştirme, yeni parça paketlerine uygun. Örtüşme, geçiş yolu, görünürlük ve karakter hareketi çok test ister. Güzel ev görüntüsü kolay, hatasız yapı sistemi zor; ilk tercih olmamasının temel nedeni budur.

### 9. Duygu Pikniği

- **Hikâye / oyuncu:** farklı istekleri olan arkadaşlar piknik kurar; sosyal canlandırmayı seven 4–6 yaş, ebeveynle ortak oyun seçeneği.
- **Ana karakter / dünya:** çekingen kirpi ev sahibi; tek piknik alanı ve kısa karşılaşmalar.
- **Temel oynanış / aktiviteler:** durumu gözle → nesne veya yaklaşım seç → karakterin tepkisini izle. Yer paylaşma, sessiz köşe hazırlama, birlikte etkinlik seçme.
- **Eğlence / öğretici unsur:** komik ve sıcak karşılaşmalar; farklı ihtiyaçları konuşma fırsatı. Tek doğru duygu cevabı veya klinik gelişim iddiası yok.
- **Görsel yön:** anlaşılır beden dili, sakin mimikler ve düşük arayüz yoğunluğu.
- **Fark / tekrar:** duygu kartı sınavı yerine aynı durumun farklı kabul edilebilir sonuçları.
- **Zorluk / kapsam:** orta; dört karakter, sekiz durum, 16–20 kısa tepki; **14–22 hafta**.
- **Ticari potansiyel / risk:** ebeveyn açısından anlaşılır tema; çocuk için eğlence değeri ayrıca gösterilmeli. Didaktik anlatı, kültürel duygu yorumları ve yetişkin ipucuna bağımlılık riski var. Yeni karşılaşmalarla genişler; gelişim uzmanı içerik incelemesi bütçelenmeli.

### 10. Renk Mutfağı

- **Hikâye / oyuncu:** bir renk atölyesinde boyalar, desenler ve hayali şekil canlıları hazırlanır; görsel üretim seven 4–6 yaş.
- **Ana karakter / dünya:** lekeli, yuvarlak bir atölye faresi; karışım tezgâhı ve sergi duvarı.
- **Temel oynanış / aktiviteler:** renk seç → karıştır → damgala → ürettiğin canlıyı hareket ettir. Renk oranları için büyük, az sayıda seçenek.
- **Eğlence / öğretici unsur:** görünür dönüşüm ve kişisel eser; renk, şekil, örüntü ve deneme. Sonuçlar tutarlı olur; gerçek pigment fiziği öğrettiği iddia edilmez.
- **Görsel yön:** krem arka plan, temiz renk kapları ve belirgin dokulu baskılar.
- **Fark / tekrar:** hazır boyama sayfası yerine canlandırılan özgün üretimler ve yerel sergi.
- **Zorluk / kapsam:** düşük-orta; üç araç, altı temel renk, 12 damga ve 12 kayıt yuvası; **10–16 hafta**.
- **Ticari potansiyel / risk:** içerik azlığına rağmen kombinasyonla çeşitlilik; erişilebilir üretim kapsamı. Boyama kategorisi kalabalık, düşük farklılaşma ve yetişkin için “başka bir çizim uygulaması” algısı riski var. Dokular ve yeni araçlarla genişleyebilir.

## 9. Ağırlıklı değerlendirme

Her hücre 1–10 arası **analitik görüş**. Toplam = Σ(puan × ağırlık / 10). Eğlenme %20; ticari potansiyel %20; tekrar %15; görsel %10; ebeveyn ödeme isteği %10; eğitim %10; geliştirme kolaylığı %10; fark %5. Kolaylık puanı yükseldikçe üretim daha kolaydır.

| Konsept | Eğlence 20 | Ticari 20 | Tekrar 15 | Görsel 10 | Ebeveyn 10 | Eğitim 10 | Kolaylık 10 | Fark 5 | /100 |
|---|---:|---:|---:|---:|---:|---:|---:|---:|---:|
| Minik Dostlar – Yağmur Bahçesi | 9 | 8 | 9 | 9 | 9 | 8 | 6 | 8 | **83,5** |
| Yaprak Postası | 8 | 7 | 7 | 8 | 8 | 8 | 7 | 8 | 75,5 |
| Bulut Ustası | 9 | 7 | 9 | 9 | 8 | 9 | 7 | 9 | **83,0** |
| Cep Orkestrası | 9 | 6 | 9 | 8 | 8 | 8 | 7 | 8 | 78,5 |
| Tamirci Pati | 8 | 8 | 8 | 8 | 9 | 9 | 8 | 8 | **82,0** |
| Gölge Masalları | 8 | 6 | 9 | 9 | 8 | 8 | 6 | 9 | 77,0 |
| Yuvarlak Yollar | 8 | 7 | 7 | 8 | 8 | 9 | 8 | 6 | 76,5 |
| Minik Ev Mimarları | 9 | 7 | 9 | 9 | 8 | 8 | 5 | 7 | 79,0 |
| Duygu Pikniği | 7 | 7 | 7 | 8 | 9 | 8 | 7 | 8 | 74,5 |
| Renk Mutfağı | 8 | 6 | 8 | 9 | 8 | 8 | 9 | 6 | 77,0 |

**Gerekçeler:** Yağmur Bahçesi karakter bağı ve serbest oyunla güçleniyor, birleşen kurallar kolaylığı düşürüyor. Bulut Ustası'nın dönüşümü görsel ve eğitsel açıdan güçlü; ödeme tezi ile karakter bağı daha belirsiz. Tamirci Pati hedef anlaşılabilirliği, üretim kontrolü ve somut neden-sonuçla yükseliyor. Yaprak Postası'nın tekrar değeri rota içeriğine bağlı. Cep Orkestrası'nın yeniden birleştirme değeri yüksek, ses uzmanlığı ve dar alıcı ilgisi riski var. Gölge Masalları yaratıcı ama çocuk editörü zor. Yuvarlak Yollar iyi öğrenme fırsatı sunsa da güçlü bulmaca rekabeti var. Ev Mimarları özgür ama teknik olarak en riskli. Duygu Pikniği ebeveyne güçlü, çocuğun serbest eğlencesine daha belirsiz. Renk Mutfağı kolay ve görsel, farklılaşması zayıf.

Bu sayılar talep ölçümü veya istatistiksel sıralama değildir. 83,5 ile 83 arasında anlamlı üstünlük kanıtı yok. Örneğin Yağmur Bahçesi'nin ticari puanını 8'den 7'ye düşürmek toplamını 81,5 yapar. İlk üç birbirine yakın; tercih risk ve test sonuçlarıyla değişebilir.

## 10. İlk üç ve birinci tercihin gerekçesi

| Aday | Seçme nedeni | Vazgeçme / daraltma koşulu |
|---|---|---|
| **Yağmur Bahçesi** | Hayvan bağı, küçük kişisel alan, görsel kimlik ve tekrar birleşimi; ilk ada fikrine uygulanabilir ölçek | Çocuklar yalnız rastgele dokunup ilişkileri fark etmiyorsa veya tekrar aynı hareketle sınırlıysa kapsam genişletilmez |
| **Bulut Ustası** | Daha az karakterle güçlü görsel sonuç; etkileşim araçları tekrar kullanılabilir | Hava araçları üç düğmelik gösteri gibi kalıyor, yeni deneme doğurmuyorsa yeniden tasarlanır |
| **Tamirci Pati** | En öngörülebilir üretim kapsamı; somut hedef ve hata toleransı | Parça yerleştirme bir çalışma kâğıdı gibi hissediliyor ve alternatif çözümler eğlendirmiyorsa değişir |

**Birinci tercihim Yağmur Bahçesi; yalnız en yüksek puanı aldığı için değil.** Özgün marka yaratma isteğiyle tek maskot üzerinden bağ kurabilmeyi, oyuncunun bıraktığı alanı sahiplenmesini ve üç küçük araçla birçok tepki üretebilmeyi birlikte sunuyor. Ancak bir serbest inşa oyunu kadar büyük olmamalı. Takvim 12–16 haftaya kesin biçimde sıkışırsa Tamirci Pati veya Renk Mutfağı'nın çok dar kapsamı daha gerçekçi olur; aynı kaliteyle geniş bahçeyi bu süreye sıkıştırmayı vaat etmiyorum.

## 11. “Dokununca yaşayan dünya” değerlendirmesi

Çiçeğin açılması veya hayvanın zıplaması tek başına pahalı değil. Maliyeti artıran, her nesne için farklı animasyon/ses üretmek ve bir etkileşimin diğer durumlarla çakışmasını çözmek. Örneğin dört karakter × altı nesne × üç hava durumu 72 olası bağlam yaratabilir; bunların hepsine özel animasyon vermek gereksizdir. Ortak birkaç tepki ve elle seçilmiş anlamlı kombinasyon daha yönetilebilir.

Tahmini bir karşılaştırma: 12 bağımsız dokun-tepki noktası birkaç haftalık uygulama/polish işi olabilir; kalıcı kayıt, sürükleme, karakter ilişkisi ve çoklu sonuç eklemek işi aylar ölçeğindeki ürünün parçası yapar. Bunlar hazır sanat ve deneyim varsayımına bağlı tahminlerdir. Tek bir genel süreyle “ucuz sistem” demek doğru olmaz.

Eğlence ölçütü animasyon sayısı değil: çocuk aynı aracı başka nesnede deniyor mu, sonucu tahmin edip tekrar değiştiriyor mu, kendi hikâyesini kuruyor mu? Daha önce gördüğü tepkiyi yeniden izlemek de değerli olabilir; ancak buna dayanarak uzun süreli tekrar garantisi verilemez.

## 12. Görsel ve ticari potansiyel

| Yaklaşım | Avantaj | Maliyet / risk | Bu aşamadaki öneri |
|---|---|---|---|
| A. Pastel 2D | En kolay okunur siluet, düşük teknik yük, hızlı varyasyon | Sıradan stok illüstrasyon görünümü; derinlik daha sınırlı | Zaman daralırsa güçlü seçenek |
| B. 2D karakter + 2.5D dünya | Oyuncak maketi hissi; tek açıyla tutarlı sahne; mağaza görseli güçlü olabilir | Örtüşme, perspektif, dokunma hedefleri ve arka/ön sıralaması | **İlk görsel deneme için tercih** |
| C. Stilize 3D | Dönüş ve ışıkta esneklik | Model, rig, animasyon, teknik sanat ve cihaz maliyeti | İlk küçük sürüm için önerilmez |

B yaklaşımını dönen gerçek 3D ada olarak değil, sabit kamera ve katmanlı sprite derinliği olarak sınırlamak önerim. İzometrik açı çocukların nesneye erişimini zorlaştırırsa daha önden görünen 2.5D açıya geçilir. URP ve ışık sistemi seçimi görsel prototipte kanıtlanmadan kilitlenmez.

Görsel potansiyel: tek bir güçlü maskot, okunabilir hayvan ifadeleri, dokununca değişen büyük öğeler ve birbiriyle uyumlu yüzeyler. Kaliteyi yüzlerce dekor yerine az sayıda özgün hareket, tutarlı gölge ve iyi ses sağlar. Kesin palet, karakter adayları ve sahne çizimleri konsept onayından sonra hazırlanacak; şu anda üretilmiş oyun ekranı yok.

Ticari potansiyel: 10–15 saniyelik tanıtımda “dokun, bahçe değişsin, dostun tepki versin” vaadi gösterilebilir. Ebeveyn sayfasında tek ödemenin kapsamı, çevrimdışı kullanım ve veri yaklaşımı açık anlatılabilir. Bunlar satış garantisi değil, gösterilebilir bir ürün vaadi.

## 13. Fiyat ve ticari model için ilk çerçeve

Erişim tarihi 1 Kasım 2026; aşağıdaki ürünler aynı içerik kapsamına sahip değil. Kampanya, bölge, eski SKU ve kullanıcıya özel teklif farkları olabilir.

| Referans | Görünen fiyat / model | Kullanım sınırı |
|---|---|---|
| Toca Boca World, ABD | Ücretsiz + içerik; ör. Modern Mansion 2,99 USD, Character Creator 4,99 USD | Tam oyun fiyatı sayılmaz. [Kaynak](https://apps.apple.com/us/app/toca-boca-world-game-play/id1208138685) |
| Sago Mini World, ABD | Çeşitli planlar; 7,99 USD “World Plan” listeleniyor | Ürün adından dönemini kesin çıkarmıyorum. [Kaynak](https://apps.apple.com/us/app/sago-mini-world-kids-games/id874425722) |
| Pok Pok, ABD | Aylık 6,99/9,99/12,99; yıllık 45,99–79,99; Lifetime 89,99 USD ürünleri | Hepsi her kullanıcıya sunuluyor varsayılmadı. [Kaynak](https://apps.apple.com/us/app/pok-pok-montessori-preschool/id1550204730) |
| Bimi Boo örnek ürünü, ABD | 5,99 USD aylık, 39,99 USD yıllık ürün | Bütün Bimi Boo uygulamalarına genellenmez. [Kaynak](https://apps.apple.com/us/app/kids-learning-games-stories/id1537964112) |
| LEGO DUPLO World, ABD | Aylık 6,99; yıllık 29,99/39,99; çeşitli paketler 4,99 USD | Birden çok model/SKU birlikte. [Kaynak](https://apps.apple.com/us/app/lego-duplo-world/id1458749093) |
| Thinkrolls School Edition, ABD | 89,99 USD tek alım | Geniş okul/ömür boyu paket; küçük MVP için doğrudan fiyat emsali değil. [Kaynak](https://apps.apple.com/us/app/thinkrolls-school-edition/id1571455832?platform=ipad) |
| Little Kitten, Birleşik Krallık | Ücretsiz başlangıç; çeşitli Unlimited/indirim planları | Bu sayfadan ABD/Türkiye fiyatı türetilmedi. [Kaynak](https://apps.apple.com/gb/app/little-kitten-cat-games/id1171095810) |
| Pocket Camp Complete, ABD | 19,99 USD tek alım | Büyük marka ve kapsamlı ürün. [Kaynak](https://ac-pocketcamp.com/en-US) |

**Bizim ilk model önerimiz:** ücretli indirme; tek ödemeyle o sürümün vaat edilen bütün içeriği. 3,99–5,99 USD aralığı yalnız ebeveyn görüşmelerinde karşılaştırılacak **fiyat hipotezi**. Türkiye fiyatı kur çarpımıyla belirlenmemeli; güncel mağaza fiyat basamakları ve yerel ödeme isteği ayrıca incelenmeli. Bu aşamada TL fiyatı doğrulanmadı.

Ücretsiz demo + ebeveyn alanından bir defalık tam sürüm açma, denenmeden ödeme engelini azaltabilir; buna karşılık satın alım geri yükleme ve kilitli içerik deneyimi ekler. İlk seçenek olarak ücretli indirme daha sade. Ücretli genişleme ancak temel ürün kendi başına doyurucu olduğunda düşünülebilir. Abonelik, reklam ve sürekli ödeme önerilmiyor.

Apple Small Business Program'a uygunluk/kabul halinde ücretli uygulama ve IAP için %15 komisyon; Google'ın %15 programına kayıtlı geliştiricide yıllık ilk 1 milyon USD gelir için %15 açıklanıyor. Bölge ve sözleşme koşulları sonucu değiştirebilir; indirim otomatik kabul edilmedi. [Apple](https://developer.apple.com/app-store/small-business-program/) · [Google](https://support.google.com/googleplay/android-developer/answer/112622?hl=en)

### Tahmini gelir duyarlılığı — satış tahmini değildir

Varsayım: ilk 12 ayda mağaza kesintisi öncesi **vergi/iade sonrası ortalama satış tabanı 4 USD**, komisyon %15, pazarlama ve üretim henüz düşülmemiş. Bu 4 USD, etiket fiyatı değildir. Hesap: adet × 4 × 0,85.

| Senaryo etiketi | Varsayımsal ücretli satış | Mağaza sonrası tutar |
|---|---:|---:|
| Düşük | 250 | 850 USD |
| Orta | 2.000 | 6.800 USD |
| Yüksek | 10.000 | 34.000 USD |

Bunlar gerçekleşme olasılığı atanmış tahminler değil, birim ekonomiyi gösteren senaryolardır. “Yüksek” organik erişim, iyi mağaza dönüşümü veya başarılı tanıtım gerektirir; bunlara dair verimiz yok. %30 kesinti varsayımında aynı tutarlar 700 / 5.600 / 28.000 USD olur. İşçilik, gelir vergisi, muhasebe, müşteri desteği, cihaz ve tanıtım çıkınca kalan kâr daha düşüktür.

Örneğin 2.000 USD dış nakit giderini, satış başına 3,40 USD katkıyla karşılamak için yaklaşık **589 satış** gerekir; geliştiricinin emeği bu hesapta ücretsiz sayılmıştır. Ek 10.000 USD emek maliyeti varsa toplam 12.000 USD için yaklaşık 3.530 satış gerekir. Reklamla edinme maliyeti 3,40 USD'yi aşarsa bu varsayımla satış başına zarar oluşur.

Yayın hesabı tabanı: Google Play 25 USD bir defa; Apple Developer 99 USD/yıl veya mevcut yerel karşılığı. Vergi ve başka masraflar hariç. [Google kayıt](https://support.google.com/googleplay/android-developer/answer/6112435?hl=en) · [Apple üyelik](https://developer.apple.com/programs/whats-included/)

Sanat/ses/dış inceleme için 500–2.000 USD, yedek bütçe için 200–500 USD yalnız **erken planlama aralıklarıdır**, tedarikçi teklifi değildir. Mac/cihaz temini, ücretli araç kullanımı, hukuk ve muhasebe ayrıca fiyatlandırılmalı. Gerçek bütçe seçilen görsel kaliteye göre bu aralığı aşabilir; küçük bütçede kapsamı azaltmak kaliteyi korumanın temel aracıdır.

Pazarlama ilkesi: ebeveynlere yönelik dürüst oyun videosu, Türkçe/İngilizce mağaza metni, bağımsız inceleme ve izinli tanıtım. Çocuklara baskı yapan satın alma çağrıları, ödüllü yorumlar ve sahte değerlendirme yok. Türkiye ve İngilizce konuşulan pazarlarda ayrı ebeveyn görüşmesi yapılmadan fiyat/mesaj başarısı genellenmez. Organik keşif de zaman ve içerik üretimi maliyeti taşır.

## 14. Güvenlik ve yayın koşullarının tasarıma etkisi

Google'da hedef kitlesine çocukları dahil eden uygulamalar Families gereklerine tabidir. Yaş seçimi ve veri beyanı gerçek ürün davranışıyla tutarlı olmalıdır. [Google hedef kitle kuralları](https://support.google.com/googleplay/android-developer/answer/9867159?hl=en)

Apple Kids kategorisi dış bağlantı ve satın alma gibi alanların ebeveyn kapısında tutulmasını ister; üçüncü taraf analitik/reklam için ciddi sınırlamalar vardır. Ebeveyn kapısı, kişisel veri işlemek için hukuken geçerli ebeveyn rızasıyla aynı şey değildir. [App Review 1.3 ve 5.1.4](https://developer.apple.com/app-store/review/guidelines/)

4–6 hedefi Apple'ın “5 ve altı” ile “6–8” yaş bantlarının sınırını kesiyor. Hangi kategori bandının seçileceği içerik ve kullanılabilirlik testine göre kararlaştırılmalı; 4+ derecesiyle karıştırılmamalı. [Apple çocuk deneyimleri](https://developer.apple.com/kids/)

COPPA'nın 2025 değişiklikleri için genel uyum tarihi 22 Nisan 2026 olarak yayımlandı; bazı hükümlerin geçişi farklı. Dolayısıyla 2026 ürünü eski bir kontrol listesine dayanamaz. Çocuklardan kişisel veri toplanması halinde kapsam, rıza, saklama ve üçüncü taraf yükümlülükleri ayrıca değerlendirilir. [Resmî düzenleme](https://www.govinfo.gov/content/pkg/FR-2025-04-22/html/2025-05904.htm)

“GDPR-K” ayrı bir tek kanun değil, GDPR'nin çocuklara ilişkin hükümlerini anlatan kullanım. Rızaya dayalı, doğrudan çocuğa sunulan çevrimiçi hizmetlerde ebeveyn rızası eşiği ülkeye göre 13–16 arasında değişebilir. Bu bir oyuna erişim için genel yaş yasağı değildir. [Avrupa Komisyonu](https://commission.europa.eu/law/law-topic/data-protection/information-business-and-organisations/legal-grounds-processing-data/are-there-any-specific-safeguards-data-about-children_en)

KVKK'nın mobil mahremiyet rehberi çocuk verilerini ayrıca ele alıyor. Türkiye için işleme hukuki dayanağı, aydınlatma, veri aktarımı ve temsil/rıza meseleleri ürünün gerçek veri akışı üzerinden incelenmeli. Burada yabancı yaş eşikleri Türkiye'ye aynen uygulanmadı. [KVKK rehberi](https://www.kvkk.gov.tr/SharedFolderServer/CMSFiles/8ba209bb-fa93-4479-84f0-dd55aac97a0f.pdf)

Ürün önerisi: çocuktan hesap/ad/doğum tarihi istememe, cihazda yerel oyun kaydı, reklam/sohbet/bildirim eklememe, çekirdekte ağ çağrısı gerektirmeme. Bu mimari yükü azaltır; “hiç veri toplamıyoruz” beyanı ancak bütün SDK ve ağ davranışı kontrol edildikten sonra yazılabilir. Mağaza ödeme altyapısı, destek e-postası ve tanıtım sitesi ayrı veri akışları yaratabilir. Nihai gizlilik metni ve hedef pazar uygunluğu uzman incelemesi gerektirir; burada uygunluk garantisi verilmiyor.

## 15. Konsept doğrulama ve sonraki aşama

Önce küçük örnek, sonra içerik genişletme. Başlangıç metnindeki 4 karakter/6 aktivite/20+ animasyon ticari sürüm hedefi olarak tartışılabilir; ilk doğrulama için gereğinden büyük. Yağmur Bahçesi seçilirse önce bir karakter, bir alan, üç bağlantılı etkileşim ve kısa geri dönüş deneyimi sınanmalı. Ticari MVP kapsamı ancak bu bulgularla kesinleşmeli.

### Doğrulama planı — önerilen eşikler

| Deneme | Ölçülecek şey | İlk karar eşiği; sektör standardı değildir |
|---|---|---|
| Konsept görüşmesi | 12 ebeveyne üç fikir, kapsam ve fiyat hipotezi | Tercih nedenlerini öğren; küçük örneklemden pazar payı çıkarma |
| Görsel yön | 4–6 yaştan, ebeveyn izniyle iki çizim yaklaşımı | Karakter/etkileşim hedefi anlaşılmıyorsa çizimi değiştir |
| İlk 30 saniye | 8 çocukta ilk bağımsız anlamlı eylem | En az 6/8; aksi halde başlangıcı düzelt |
| İlk beş dakika | Üç ana etkileşimi yetişkin adımı olmadan bulma | En az 6/8; 4, 5 ve 6 yaş dağılımını ayrıca kaydet |
| Dokunma | Yanlış hedef, başarısız sürükleme ve yardım ihtiyacı | Tekrarlayan aynı hata varsa hedef/gesture değişsin |
| Tekrar değeri | Başka gün tarafsız davetle yeniden açma ve farklı kullanım | En az 5/8 gönüllü dönüş ön sinyal; zorunlu veya ödüllü dönüş sayılmaz |
| Ebeveyn algısı | İçerik/fiyat anlaşılır mı, güven ve değer gerekçesi ne? | Sorunu açıkça anlatamayan veya yanıltıcı bulan varsa metni düzelt |
| Teknik | Kayıt/geri yükleme, arka plan, uçak modu, ses kapalı | Kritik veri kaybı/çökme sıfır; gerçek cihazlarda tekrar doğrula |

Çocuklar rahatça bırakabilir; test boyunca ebeveyn izin ve gözetimi sağlanır. İsim/fotoğraf/ses toplamadan anonim gözlem notları yeterli olabilir. Gereksiz kayıt alınmaz. Bu testler gelişim testi veya eğitimsel başarı araştırması değildir. Çocukların isteğini “daha uzun ekran süresi” metriğine çevirmemek gerekir.

### Önemli riskler

| Risk | Erken belirti | Karşılık |
|---|---|---|
| Özgür keşif amaçsız geliyor | Çocuk kısa sürede ne yapacağını soruyor | İsteğe bağlı görsel davet ve daha belirgin sonuç |
| Ücretli indirme dönüşümü düşük | Ebeveyn içerik değerini göremiyor | Vaat/kapsam/görseli düzelt; gerekirse ayrı demo modelini değerlendir |
| Görsel kalite düşük | Karakterler/çevre birbirine ait görünmüyor | İçeriği azalt, sanat yönü ve animasyona bütçe ayır |
| Kapsam şişiyor | Her nesne ayrı sistem/mini oyun istiyor | Üç çekirdek etkileşim kuralına geri dön |
| Kayıt veya performans sorunu | Geri dönüşte düzen kaybı, dokunmaya gecikme | Yeni içerik eklemeden düzelt ve cihazda doğrula |
| Marka benzerliği | İsim veya maskot mevcut markayla karışıyor | Adı/karakteri değiştirmek; seçimin ardından TÜRKPATENT/WIPO/EUIPO ve hedef mağazalarda araştırma |
| iOS erişimi/bütçe | Mac, imzalama veya cihaz yok | Windows'ta geliştirme sürer; Mac/bulut ve cihaz bütçesi yayın takviminden önce netleşir |

### Onaya sunulan somut karar

**Öneri:** “Minik Dostlar – Yağmur Bahçesi” ile bir sonraki **konsept ayrıntılandırma ve görsel yön** aşamasına geçmek. Bu aşamanın teslimi: temel oyun döngüsü ve ilk beş dakika taslağı; beş maskot adayı; iki görsel yaklaşım karşılaştırması; seçilen yaklaşım için karakter/alan/UI konseptleri ve dar prototip kapsamı. Bunlar onayından sonra GDD ve Unity geliştirme adımlarına temel olacak.

Bu onay, mağaza yayınına, ücretli asset satın almaya veya bilgisayarda kurulum değiştirmeye verilmiş genel izin sayılmayacak. Unity projesi ve ilk teknik örnek, kendi somut kapsamı sunulduktan ve geliştirme aşaması onaylandıktan sonra oluşturulacak. Kullanıcının ana metnindeki aşamalı onay kuralı bu sınırın kaynağıdır.

**Karar sorusu:** Yağmur Bahçesi ile konsept ve görsel yön aşamasına geçelim mi, yoksa Bulut Ustası veya Tamirci Pati'yi mi seçiyorsun?

## 16. Bu aşamanın doğrulama durumu

- Yerel Unity kurulumları ve modül klasörleri incelendi; sonuçları ayrı raporda.
- Açık kaynaklı raporlar, mağaza/geliştirici sayfaları ve sınırlı görünür yorumlar üzerinden masa başı araştırma yapıldı.
- On konsept, istenen sekiz ağırlıkla hesaplandı; puanlar öznel analitik değerlendirme olarak işaretlendi.
- Rakiplerin ilk beş dakikası cihazda oynanarak test edilmedi. Çocuk/ebeveyn testi yapılmadı; ödeme isteği ve eğitimsel etki doğrulanmadı.
- Unity çalıştırma, lisans, oyun derlemesi ve mobil build testi yapılmadı. Bu belge ve erişim raporu dışında oyun projesi üretilmedi.

Bekleyen iş bir ortam hatası değil, **konsept seçimi ve aşama onayıdır**.
