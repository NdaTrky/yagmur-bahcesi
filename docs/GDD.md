# Yağmur Bahçesi — Game Design Document

Temel tasarım kaydı 0.1 · 2 Kasım 2026. Aşağıdaki metin ilk prototipin kapsamını korur. **Güncel 0.2 kapsamı, ekonomi, koleksiyon ve mini oyunlar [GENISLEME_02.md](GENISLEME_02.md) belgesindedir ve burada geçen üç yatak/dört dost/para birimi yok gibi eski kapsam ifadelerinin yerini alır.**

## Executive summary ve kapsam kararı

Yağmur Bahçesi, 4–6 yaş çocukların küçük bir bahçeyi dokunarak değiştirdiği, hayvanlarla oynadığı ve kendi pikniğini kurduğu sakin bir keşif oyunudur. Temel vaat: “Küçük dokunuşlar, kocaman keşifler.” Zaman sınırı, başarısızlık ekranı, para birimi ve günlük görev yoktur.

Kullanıcı 2 Kasım 2026'da konsepti, geliştirmeyi ve aşamalar tamamlandığında yayını onayladı. Aynı gün verdiği yönlendirmeyle ilk platform **yalnız Android** oldu. iOS ertelendi. Bu değişiklik araştırma belgesindeki önceki iki platform kapsamının yerini alır. Araştırma belgesi tarihsel kayıt olarak korunur.

İlk teknik sürüm 0.1, ticari olarak tamamlanmış ürün değildir. Kod ve masaüstü doğrulamasından sonra gerçek Android cihazları, çocuk/ebeveyn kullanılabilirlik oturumları, ürün kimliği ve mağaza koşulları tamamlanmadan genel yayına çıkılmaz.

## Pazar, hedef kitle ve konumlandırma

Kaynaklı pazar incelemesi, 13 rakip/referans, 10 fikir ve ağırlıklı puanlar [araştırma belgesinde](../ARASTIRMA_VE_KONSEPT_ONAYI.md) bulunur. En yakın referanslar Pok Pok ve Sago Mini; kişisel mekân ve rol yapma için Toca Boca World; maskot tepkileri için Little Kitten. Rakip karakterleri, ikonları ve görsel düzenleri kullanılmaz.

Birincil oyuncu okumayı henüz öğrenmemiş veya yeni öğrenen 4–6 yaş çocuktur. Satın alma kararını ebeveyn verir. Tek cihazda birlikte oyun desteklenir; çevrimiçi çok oyunculu sistem bulunmaz. Reklamsızlık ve çevrimdışı kullanım farklılaşmanın tamamı değil, temel ürün şartıdır. Asıl fark, küçük bir alandaki hayvanlar ve nesneler arasında anlaşılır ilişkiler kurulmasıdır.

Pazar fırsatı henüz doğrulanmış satış açığı değildir. Tek ödeme karşılığında güvenilir, saklanan, okunaksız menülerle uğraştırmayan bir oyuncak dünya sunma hipotezidir. Türkiye ve İngilizce konuşulan pazarlardaki ödeme isteği ayrı araştırılır.

## Hikâye ve dünya

Mina ve üç dostu aynı bahçeyi paylaşır. Çocuk onlara hükmeden görev yöneticisi değil, bahçeye dokunup yeni oyunlar çıkaran bir arkadaştır. Bir fidanın büyümesi, yaprağın suya ulaşması ve bir meyvenin paylaşılması küçük olaylardır. Sinematik, uzun diyalog ve yazılı görev zinciri yoktur.

Harita tek yatay sahnedir. Sol tarafta üç toprak yatağı, sağda gölet, alt ortada piknik örtüsü bulunur. Aralarındaki boş çimen oyuncak taşıma alanıdır. Ağaç, küçük kulübe ve çevre bitkileri mekân duygusu sağlar; bütün arka plan ayrıntılarının etkileşimli olduğu iddia edilmez. Etkileşimli nesneler daha büyük ve hareketlidir. Dönen kamera veya serbest harita kaydırma yoktur.

## Beş maskot adayı ve seçim

| Aday | Tür / kişilik | Renk ve görünüm | Hareket / ses | Rol ve karar |
|---|---|---|---|---|
| Mina | Tavşan; meraklı ve nazik | Krem gövde, yaprak biçimli uzun kulak, adaçayı fular | Küçük zıplama, iki yumuşak nota | Ana maskot; silueti güçlü, kulakları ifadeye uygun |
| Pufi | Kirpi; sakin, paylaşımcı | Bal rengi, yuvarlak kahverengi dikenler, minik çanta | Kısa sevinç sallanması, sıcak tok nota | Piknik dostu; yardımcı karakter |
| Luli | Ördek; oyunbaz | Tereyağı sarısı, turuncu gaga, yaprak şapka | Suyun yanında küçük sıçrama, hafif tiz nota | Gölet dostu; yardımcı karakter |
| Kori | Sincap; düzenlemeyi seven | Tarçın gövde, krem karın, kıvrık büyük kuyruk | Kuyruklu gövde salınımı, kısa yükselen aralık | Dekor ve paylaşım dostu; yardımcı karakter |
| Sumi | Salyangoz; sabırlı gözlemci | Toz mavi kabuk, şeftali gövde | Yavaş uzanma, uzun yumuşak ton | Ana maskot için daha az okunur ifade; ilk sürüme alınmadı |

Mina seçimi prototip kararıdır; çocuk testiyle değişebilir. Bu adların ticari marka araştırması tamamlanmış değildir. Yerel paket kimliğinde geçici `.dev` eki kullanılır; yayıncı alan adı ve uygulama kimliği mağaza yüklemesinden önce sabitlenir.

## Sanat yönetimi

Seçilen yaklaşım: sabit açılı 2.5D boyalı çevre ve 2D karakter sprite'ları. Tam 3D model/rig yok. Gouache ve suluboya hissi, yumuşak doku, açık yüzler ve sıcak doğal ışık kullanılır. Arka plan daha ayrıntılı, etkileşimli nesneler daha iri ve temiz siluetlidir. Arka plan yoğunluğu cihaz testinde okunabilirliği azaltırsa sadeleştirilir.

| İşlev | Renk |
|---|---|
| Ana UI zemini | Krem `#FCF9E8` |
| Ana metin | Koyu yosun `#385240` |
| Birincil buton | Adaçayı `#577A54` |
| Seçili araç | Açık yaprak `#D4E6B8` |
| Sıcak vurgu | Tereyağı `#F4CD73` |
| İkincil vurgu | Kayısı `#E6A878` |
| Su ve hava | Toz turkuaz `#87BFC8` |

Baş oranı gövdeye göre büyüktür; küçük koyu gözler, kısa burun ve anlaşılır beden dili kullanılır. İnce siyah kontur, parlak neon ve sert parlamalar kullanılmaz. Işık çoğunlukla çizime gömülüdür; gerçek zamanlı pahalı gölgeler yoktur. Karakterlerin çevreyle temas hissi, yerleşim ve hafif yer gölgesiyle sağlanır.

Üretilen görseller `YagmurBahcesi/Assets/RainGarden/Art/` altında: `Garden.png` çevre, `Friends.png` dört hayvan × dört ifade, `Objects.png` on iki araç/nesne. Üretim yerleşik image_gen ile yapıldı; tam talimatlar [ART_PROMPTS.json](ART_PROMPTS.json) dosyasında. Bunlar yapay zekâ çıktısıdır; özgünlük ve hak incelemesi mağaza öncesi ayrıca yapılır. Kaynak dosyalar korunur; Unity içe aktarma aşaması atlas dilimlerini oluşturur.

## İlk açılış ve ilk beş dakika

Ana menüde oyunun adı, Mina ve büyük bir oyun butonu vardır. Dil/ses kontrolleri yetişkin için görünür; oyun için metin okumak gerekmez. Giriş, hesap veya ağ bekleme ekranı yoktur.

İlk 30 saniye hedefi: bahçeye girme, bulutu veya toprağı fark etme, ilk dokunuşta yağmur ve filiz görme. Bu bir doğrulama hedefidir; çocuklarda ölçülmüş sonuç değildir. Öğretici video gösterilmez. Kısa isteğe bağlı ipucu ve yardım paneli vardır; görsel anlatım tek başına yeterli değilse kısa TR/EN sesli yönlendirme sonraki kalite adımıdır.

İlk beş dakika olası akış: toprak → yağmur → filiz → güneş/ikinci yağmur → çiçek; sonra yaprak → gölet → rüzgâr; ardından meyve → piknik. Sıra zorunlu değildir. Çocuk bir alanda kalabilir, hayvanlara dokunabilir veya dekor taşıyabilir. Oyun sonunda seans puanı veya yeni görev baskısı verilmez.

## Temel döngü ve ilerleme

Fark et → dokun veya taşı → sonucu gör/dinle → başka bir şey dene → kendi düzenine dön. Tüm araçlar baştan kullanılabilir. Kilit açma, yıldız, enerji, günlük ödül ve süreli içerik yoktur. İlerleme, filizlerin çiçek oluşu, yaprağın yüzmesi, piknikteki meyveler ve dekorların yeridir.

Yerel kayıtta üç yatağın büyüme/renk durumu, teknenin suya bırakılması, dört piknik meyvesi, üç dekor konumu ve yetişkin ayarları saklanır. İlk sürümde hayvanların taşınmış konumları kalıcı değildir; yeniden açılışta başlangıç bölgelerine dönerler. Oyunun gelecekteki sürümleri kayıt sürümü ve göç kuralı olmadan eski veriyi bozmamalıdır.

## Oyun mekanikleri

### Yağmur ve çiçekler

Toprak yatakları üç durum taşır: tohum, filiz, çiçek. Yağmur bir adım büyütür. Güneş filizi çiçeğe dönüştürür; kuru toprağı tek başına büyütmez. Keşif aracında toprağa dokunmak da yağmur etkileşimine erişim sağlar; sürüklemeyi yapamayan çocuk engellenmez. Açmış çiçeğe dokunmak iki renk arasında değişim verir. Tekrar sulamak zarar vermez.

Bulut sürüklenebilir; yatağın yakınına bırakmak yağmuru oraya taşır. Hayvan tepki verir. Animasyonlar kısa ve düşük seslidir. Ekonomi, büyüme sayacı ve bekleme süresi bulunmaz.

### Gölet ve yaprak

Yaprak gölete taşınabilir veya dokunarak suya gönderilebilir. Suda hafifçe salınır; rüzgâr aracının etkisi birkaç saniyelik hızlanmadır. Gölete dokunmak dalga ve ördek tepkisi oluşturur. Gerçek sıvı simülasyonu yerine belirli yörünge ve görsel dalga kullanılır. Batan veya kaybolan nesne yoktur.

### Piknik ve paylaşma

Elma/armut örtüye veya hayvana götürülür; meyve kaynağı geri gelir, dört yuvalı örtüde sonuç görünür. Kaynağa dokunma alternatifi vardır. Alan dolduğunda ilk yer yenilenir; puan veya kıtlık yoktur. Piknik dört hayvanın aynı anda beslenmesini isteyen görev değildir; hayvanların açlık göstergesi bulunmaz.

### Serbest etkileşim ve dekor

Dört hayvan dokunuşa ifade ve küçük hareketle cevap verir, bahçe sınırlarında taşınabilir. Rüzgârgülü, taş yığını ve mantar taşınabilir; konumları kaydedilir. İlk doğrulama sürümünde üç dekor bulunur. Araştırmadaki on dekor hedefi, tekrar oynama testinden sonra genişletilecek bir adaydır.

## Giriş, fizik ve erişilebilirlik

Dokunma ve fare aynı işlevlere gider. Tek bir etkin parmak izlenir; ikinci parmak sürüklemeyi çalmaz. Dokunma iptali, uygulamanın arka plana geçmesi veya odağını kaybetmesi nesneyi güvenli konuma döndürür. UI üzerindeki dokunuşlar dünyaya geçmez.

BoxCollider2D yalnız seçim alanı olarak kullanılır. Yerçekimi ve serbest düşme kapalı; çocuk nesneleri kaybetmez. Bırakma alanları nesne çiziminden daha geniştir. Kamera bütün 16:9 bahçeyi tabletlerde korur; cihaz çentiği ve güvenli alan UI'ya uygulanır. Dar ekranlarda UI yerleşimi ayrıca sınanmalıdır.

Renk tek başına yönerge taşımaz: bulut, güneş, rüzgâr ve karakter biçimleri ayrıdır. Ses kapalıyken temel sonuçlar görünürdür. Hareket azaltma seçeneği vardır; bütün efektlerin duyusal uygunluğu gerçek çocuk/ebeveyn testini gerektirir. Metinler Nunito ile Türkçe karakter destekli olarak sunulur.

## UI ve menüler

Ana menü, oyun ekranı, yardım paneli, yetişkin kapısı, yetişkin ayarları, gizlilik/bilgi paneli ve sıfırlama onayı vardır. Oyun araç çubuğunda keşif/yağmur/güneş/rüzgâr simgeleri bulunur. Oyun sırasında alışveriş veya dış bağlantı yoktur.

Yetişkin alanı rastgele iki basamaklı toplama sorusuyla açılır; bu, hukuki ebeveyn kimliği doğrulaması değildir. Alanın yanlışlıkla açılma oranı çocuk testinde ölçülür. Sıfırlama işlemi ikinci açık onay ister. Ayarlar: dil, ses, azaltılmış hareket. Geri/Android geri tuşu önce paneli kapatır, sonra ana menüye döner.

## Ses ve animasyon

Özgün yumuşak pentatonik tonlar, kısa yağmur dokusu ve düşük sesli ortam tonu üretildi. Üçüncü taraf müzik paketi yoktur. Art arda dokunuşlarda ses sayısı sınırlanır. Ses seviyeleri gerçek telefon hoparlörü ve kulaklıkla ayrıca kontrol edilmelidir. Uzun müzik döngüsü yerine sessizliğe alan bırakılır.

Animasyon kaynağı: dört hayvan için sakin, mutlu, gözleri kapalı ve meraklı pozları; kodla küçük nefes, zıplama, salınım, tekne hareketi, yağmur ve yumuşak parçacıklar. Her pozun üretim kalitesi ve geçiş hizası görsel testle incelenir. İlk sürümde karmaşık rig veya Animator durum ağı gerekmiyor; bu bilinçli kapsam kararıdır.

## Eğitimsel fırsatlar

| Alan | Mekanik | Doğal kullanım | Gözlem |
|---|---|---|---|
| Neden-sonuç | Yağmur/filiz/güneş | Çocuğun farklı araçları denemesi | İlişkiyi ikinci denemede tahmin ediyor mu? |
| Sınıflama ve renk | Çiçek varyasyonu, farklı meyveler | Kendi görsel düzenini kurması | Renk/şekli ayırıp seçiyor mu? |
| Mekânsal ilişki | Tekneyi gölete, meyveyi örtüye taşıma | Bir amaç için yer değiştirme | Hedefi yetişkin göstermeden buluyor mu? |
| Paylaşma ve canlandırma | Ortak piknik | Hayvanları içeren kendi öyküsü | Kendiliğinden sosyal anlatı kuruyor mu? |
| İnce motor kullanım | Büyük hedeflere dokunma/sürükleme | Oyunun doğal kontrolü | Tekrarlayan giriş hatası var mı? |

Bunlar terapi, gelişim ölçümü veya eğitimsel başarı iddiası değildir. Çocukların hız ve yetenekleri arasında fark beklenir; oyun puanla karşılaştırmaz.

## Teknik mimari

Unity 6000.3.17f1 mevcut kurulumuyla başlangıç yapıldı. Motor yaması yayın öncesi Android/API/16 KB uyumu bakımından yeniden gözden geçirilir. Built-in 2D SpriteRenderer kullanılır; URP, gerçek zamanlı gölge ve post-process bu sahne için gerekli görülmedi. UGUI 2.0.0 arayüzü ve yerleşik dokunmatik giriş kullanılır.

`GardenArt` ScriptableObject sprite, font ve ses referanslarını toplar. `GardenGame` etkileşimleri ve yaşam döngüsünü yönetir. `GardenEntity` seçilebilir nesne ve animasyon davranışını taşır. `GardenState/GardenRules` saf durum ve oyun kurallarını ayırır. `GardenSave` yerel JSON, geçici dosya ve yedekle kaydı yapar. `GardenUI` dil ve panelleri yönetir. `GardenBuilder` Editor içinde asset/sahne üretir ve build alır. `GardenChecks` veri/asset bütünlüğünü, `GardenSmoke` çalışan oyundaki etkileşimleri sınar.

Tek sahne yeterlidir; ağır scene-loading sistemi, sunucu, ECS, Addressables veya genel amaçlı framework eklenmedi. Animasyonlar aynı atlasları paylaşır. Geçici efektler kısa ömürlüdür; hedef cihazda GC veya kare süresi problemi görülürse havuzlama eklenir, önceden gereksiz mimari kurulmaz.

Kayıt bir geçici dosyaya yazılır, geçerli önceki kayıt yedeklenir ve ana dosya değiştirilir. Bozuk kayıt yedeği ezmemelidir. Kayıt sürümü 1; bilinmeyen sürüm güvenli varsayılana döner. Gelecekte sürüm yükseltirken geri uyum/göç tasarlanır. Uygulama kaldırılınca yerel kayıt silinebilir; bulut geri yükleme vaat edilmez.

## Android hedefi ve performans

İlk hedef Android 8/API 26 ve üstü, ARM64, IL2CPP, yatay telefon/tablet. Mağaza hedef API'si 36 olarak ayarlanır. Bunlar ilk build ayarlarıdır; gerçek alt seviye cihaz ölçümü yapılmadan destek listesi ilan edilmez. Geliştirme APK'sı debug/test amaçlıdır; mağaza için imzalı release AAB gerekir.

Hedefler: orta sınıf cihazda 60 FPS, alt destek cihazında tutarlı en az 30 FPS; 100 MB altında indirme; hızlı ilk etkileşim. Henüz ölçülmemiş hedeflerdir. Son APK boyutu, RAM tepe değeri, açılış süresi ve 15 dakikalık ısı/pil davranışı kaydedilir. Kişisel veri SDK'sı eklenmeden teknik test yapılır.

## Güvenlik, gizlilik ve ticari model

Reklam, analitik, sohbet, hesap, bildirim, konum, kamera, mikrofon ve dosya depolama izni gerektiren özellikler yoktur. Android build manifesti ve ağ davranışı ayrıca doğrulanmadan mağazaya kesin “veri toplamıyor” beyanı girilmez. Yetişkin iletişim/destek ve mağaza ödeme verileri ayrı süreçlerdir.

İlk model tek ücretli indirmedir. Fiyat 3,99–5,99 USD araştırma hipotezi olarak kalır; son fiyatı kullanıcı seçer. Ücretsiz test APK'sı ücretli mağaza modelinden ayrı geliştirme dağıtımıdır. Çocuğa satın alma teklifi yoktur. Gelecek genişleme içeriği ilk paketin zorunlu tamamlayıcısı olarak satılmaz.

Komisyonlar, hesap maliyetleri, hukuki kaynaklar ve senaryo hesapları araştırma belgesindedir. İlk sürüm başarısı satış garantisi değildir. Çocuk gizliliği ve yayıncı metinlerinin gerçek işletme bilgileriyle gözden geçirilmesi gerekir.

## Asset envanteri ve genişleme sınırı

Mevcut hedef: bir çevre resmi; 16 karakter pozu; 12 prop; dört basit kod üretimi UI şekli; beş nota, yağmur ve ortam sesi; açık lisanslı font; tek sahne. Yeni sanat dosyaları kaynak dosya ve kullanım kaydı olmadan eklenmez.

Ticari sürüm için değerlendirilecek ekler: bitki/tekne kombinasyonlarında daha fazla anlamlı tepki, en fazla birkaç yeni dekor, kısa iki dil sesli ipuçları ve daha iyi karakter geçişleri. Altı ayrı mini oyun, ikinci harita, reklam, hesap, mağaza, abonelik ve 3D dönüş bu sürümün kapsamında değildir.

## Aşamalar, tamamlanma kriterleri ve tahmini takvim

| Aşama | İş / araç | Tamamlanma kanıtı | Tahmini insan takvimi ve risk |
|---|---|---|---|
| Araştırma | Kaynak ve rakip incelemesi | Kaynaklı belge, konsept onayı | Tamamlandı; talep hâlâ hipotez |
| Konsept/sanat | GDD, imagegen, lisans kaydı | Harita, atlaslar, tutarlı görsel yön | İlk taslak hazır; 1–2 hafta revizyon payı |
| Teknik temel | Unity CLI, Editor API, Git | Açılan proje, derlenen sahne | Çalışan sonuçla ölçülür |
| İlk oynanabilir | Üç alan, dokunma, UI, kayıt, ses | Çalışan build ve otomatik test | İlk sürüm geliştiriliyor; 2–4 hafta iyileştirme payı |
| Cihaz testi | Android telefon/tablet, Profiler | Kare süresi, kayıt ve giriş raporu | 1–2 hafta; cihaz erişimi gerekli |
| Kullanıcı testi | Ebeveyn izinli küçük oturumlar | 4/5/6 yaş gözlemleri ve düzeltmeler | 2–4 hafta; katılımcı takvimi gerekli |
| Ürün kalitesi | Animasyon, ses, erişilebilirlik | Kritik sorun yok; tekrar testi | 2–6 hafta, bulgulara bağlı |
| Yayın hazırlığı | İmzalı AAB, Play Console, metinler | Teknik ve mağaza listesi tamam | Hesap ve inceleme süreleri dış bağımlılık |

Bu süreler garanti değildir; tek oturumda dosya üretmek kullanıcı testlerinin yerini tutmaz. Tek geliştiricinin ticari kaliteye ulaşması araştırmada verilen aylar ölçeğindeki kapsamı gerektirebilir. Kodla geçen kontrol, eğlence veya yaşa uygunluk testinin geçtiği anlamına gelmez.

## Test, risk ve yayın stratejisi

Otomatik kontroller: büyüme kuralları, hatalı giriş, kayıt roundtrip, bozuk kayıt/yedek kurtarma, sahne referansları, gerçek çalıştırmada etkileşim zinciri, dil değişimi ve ekran yakalama. Test çıktıları `Logs/` altında tutulur. Manuel cihaz ve aile oturumları [test planında](TEST_PLANI.md) tanımlanır.

Riskler: dört yaşta sürükleme güçlüğü; ayrıntılı çevrede etkileşim hedefini bulamama; düşük içerik çeşitliliği; atlas pozları arasında boyut değişimi; alt seviye telefonda bellek; yayıncı hesabı ve marka kimliğinin eksikliği. Soruna göre önce giriş/sanat düzeltilir, sonra yeni içerik eklenir.

Yayın sırası: yerel Windows inceleme → Android test APK → gerçek cihaz/aile testi → gerekirse Play internal/closed test → imzalı release AAB → mağaza incelemesi → kontrollü üretim yayını. Genel yayın yalnız bütün gerekli kontrollerin ve hesap gerekliliklerinin tamamlanmasıyla yapılır. iOS sonraki ayrı çalışma olarak bekler.
