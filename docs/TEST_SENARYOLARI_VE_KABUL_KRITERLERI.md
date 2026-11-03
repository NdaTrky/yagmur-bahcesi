# Yağmur Bahçesi — test senaryoları ve kabul kriterleri

**3 Kasım 2026 · kaynak 0d2a5c9 · oyun 0.2.0.** Bu dosya uygulanacak test planıdır; aşağıdaki 112 testin bugün çalıştırıldığı anlamına gelmez. Gerçek sonuçlar [CSV kayıt formuna](QA_TEST_KAYIT_SABLONU.csv) yazılır. Mekaniklerin ve alanların tanımı [teknik referanstadır](TEKNIK_VE_FONKSIYONEL_DOKUMANTASYON.md). Açık sorunların Bxx numaraları [denetim kaydına](KOD_DENETIMI_VE_ACIK_KONULAR.md) bağlanır.

## Test oturumunu hazırlama

1. Kaynak commit, APK/AAB SHA-256, sürüm/kod, Unity sürümü, imza sertifikası özeti, cihaz/AVD, Android/API, ABI, sayfa boyutu, ekran/oran, dil ve test tarihini kaydet. Aynı paket adı, aynı binary demek değildir.
2. Normal oyuncu profilini kullanma. Ayrı test AVD'si veya ayrılmış test profili seç; mevcut kayıt ve albümü koru. Windows smoke geçici profil açar. Android otomasyonu yalnız RainGarden_API36 AVD'sinde uygulama verisini temizler; eski JSON kopyası albümün yedeği değildir.
3. **Temiz profil (F0):** varsayılan yeni GardenState. **İlerlemiş profil (F1):** üç mini oyunu bitirmiş, yeni çiçek alıp büyütmüş, en az bir dekor yerleştirmiş profil. **Hata profili (F2):** F1 kopyası, yalnız izole test alanında değiştirilir.
4. Kayıt bozulması, v1/v99, izin/disk hatası ve sınır değer testleri çalışan oyunun asıl JSON'una uygulanmaz. Uygulamayı durdurup test kopyasını düzenle; dosyaların önce/sonra hash'ini al. Gerçek disk doluluğu sistem diskini doldurarak oluşturulmaz; sınırlı test hacmi veya kontrollü I/O hata enjeksiyonu gerekir. Bu hata enjeksiyonlarının tamamı için hazır otomasyon **yoktur**.
5. Manuel Android testinde gerçek dokunma/ADB girişini kullan. Windows smoke UI callback çağrıları gerçek parmak, EventSystem ve Android yaşam döngüsünün yerine geçmez.
6. Her testte UI sonucu + bellekte/yeniden açılışta durum + ilgili JSON + log birlikte kontrol edilir. Dosyaya erişilemiyorsa test build'inde güvenli dışa aktarma veya Editor test koşucusu gerekir; release paketinde tanılama için korumaları gevşetme.
7. Her satırdaki beklenen sonuçların **tamamı** gerçekleşirse GEÇTİ; biri gerçekleşmezse KALDI. Ortam/araç eksikse ENGELLİ; denenmediyse ÇALIŞTIRILMADI. Kodun mevcut kusurlu davranışını tekrar üretmek kabul testini geçirmez.
8. Test sırasında yeni hata bulunca build, adımlar, ekran kaydı/log, beklenen/gerçek sonuç ve en küçük tekrar örneğini sakla. Kişisel cihaz dosyalarını hata paketine ekleme.

**Kanıt sütunu:** E = önceki Editor assertion kapsamı; W = önceki Windows işlev smoke; A = önceki Android API36 emülatör; P = önceki APK statik kontrolü; K = yalnız kod okuması/yeni test; Bxx = açık bulgu. E/W/A/P işareti bu satırın bütün alt durumlarıyla yeniden geçtiği anlamına gelmez; “kısmi” özellikle bu sınırı belirtir.

## Mevcut test kanıtı ve sınırları

| Kanıt | Gerçek sonuç | Kapsam sınırı |
|---|---|---|
| [Editor](../Artifacts/editor-tests.json) | 150 assertion geçti | 150 bağımsız kullanıcı senaryosu değildir; sahne referansı döngüleri ve saf kurallar dahil |
| [Windows 1600×900](../Artifacts/Smoke-1600x900/runtime-tests.json), [1280×960](../Artifacts/Smoke-1280x960/runtime-tests.json) | Her birinde 29 işlev assertion'ı geçti | Input modülü testte devre dışı; programlı callback/giriş çağrıları; bütün varyantlar değil |
| [Windows çıkışı 16:9](../Artifacts/Smoke-1600x900/process-exit.json), [4:3](../Artifacts/Smoke-1280x960/process-exit.json) | İkisi de -1073741819 ile başarısız | İşlevlerin geçmesi Windows testinin bütünüyle geçtiği anlamına gelmez |
| [Android](../Artifacts/Android-API36/android-tests.json) | 2 Kasım 2026, 22 kontrol geçti | X86_64/API36/4 KB; ilk mini oyun düzenleri, çoğunlukla yardım; fiziksel ARM64 cihaz değil |
| [Yetişkin alanı](../Artifacts/Android-API36/parent-tests.json) | Üç eski kontrol olumlu | 2 Kasım erken test; 0.2 genişlemesinden sonra bütün ayarlar/reset için yeni kanıt değil |
| [APK](../Artifacts/Android-Package/package-tests.json) | SDK/ABI/development/izin filtresi ve 16 KB dosya hizalaması geçti | AAB, gerçek imza sertifikası doğrulaması, 16 KB runtime ve Play yüklemesi değil |

APK hash'i bu dokümantasyon incelemesinde yeniden eşleştirildi: **20EE8B66A5D6724C4AFC7A949923231933A7468EF3CEEDA53B279D0D1656A3C4**, 29.080.853 bayt. Oyun testleri bu incelemede yeniden çalıştırılmadı.

## A. Başlangıç, ekranlar ve giriş

Ortak önkoşul: aksi yazmıyorsa F0, uygulama ön planda, etkileşim log'u temiz.

| ID | Senaryo / önkoşul ve uygulama adımları | Beklenen sonuç, gözlem ve geçme ölçütü | Önceki kapsam |
|---|---|---|---|
| T001 | Temiz kurulum; cihaz dili Türkçe; aç, ana ekrana bak, “Bahçeye gir”e bas. | Ana ekran Türkçe; başlangıç 0 pul, 6 alan, ilk üç dekor ve dört dost; Sumi yok. Giriş Playing ve visited değerlerini true yapar; kayıt oluşur. | E/W/A kısmi |
| T002 | F0, cihaz dili İngilizce ve ayrı denemede desteklenmeyen dil; aç. | Başlangıç İngilizce; desteklenmeyen sistem dili de İngilizceye düşer. Türkçe karakter bozulması/eksik font olmaz. | A İngilizce; K diğer |
| T003 | F1, uygulamayı kapat/aç; “Bahçeye gir”e bas. | Yeniden ana ekran açılır; otomatik bahçeye atlamaz. Giriş sonrası bakiye, türler, büyüme, keşif ve ayarlar geri gelir. | W/A |
| T004 | Ana ekranda görünen bahçe nesnelerine dokun ve sürükle. | BlocksWorld true; büyüme/renk/konum/ödül değişmez. Ana ekran düğmeleri çalışır. | K |
| T005 | Bahçede Menü'ye bas; yeniden “Bahçeye gir”i seç; bunu 10 kez tekrarla. | Her geçiş doğru ekranı açar; eski UI etkin kalmaz; profil değişmeden devam eder. Menüye dönüş kaydetmeyi dener. | W kısmi |
| T006 | Bahçede sırayla Bahçem, Keşifler, Anılar, yetişkin kapısı ve her mini oyunu aç; arkadaki nesnelere bas. | Modal varken dünya işlem almaz; kapatınca açılır. Kapalı modalın görünmez raycast engeli kalmaz. | A yetişkin; K diğer |
| T007 | Android Geri/Windows Escape: mini oyun içinde, diğer modalda, bahçede ve ana ekranda ayrı dene. | Modal kapanır; mini oyun korunur. Bahçede ana ekrana dönülür. Ana ekranda oyun kodunun ek bir çıkış/ödül akışı yoktur; işletim sistemi davranışı kaydedilir. | K |
| T008 | Bir çiçeğe art arda 30 kısa dokunuş; sonra UI düğmesine hızlı çift dokunuş. | Dizin hatası/çökme olmaz; büyüme 2'yi geçmez; UI tek kullanılabilir katmana döner; satın alma/ödül tekilleştirmesi korunur. | E kural; K UI |
| T009 | Nesne üzerinde kısa dokunuş, eşik altı hareket, eşik üstü sürükleme yap. Eşik max(12 px, ekran yüksekliği × 0,015). | İlk ikisi etkileşim, üçüncüsü Drop yoludur. Sürükleme sonunda ayrıca tıklama etkisi uygulanmaz. Parmak ve farenin sonuçları tutarlı olur. | W/A kısmi |
| T010 | Birinci parmakla nesne tut; ikinci parmakla başka nesneye dokun; önce ikinciyi sonra birinciyi bırak. | Dünya sürüklemesini ilk finger yönetir; ikinci parmak ilk nesneyi çalmaz; bırakınca sonraki normal dokunuş çalışır. | K |
| T011 | Bir parmakla dost/dekor tutarken ikinciyle Bahçem veya etkinlik modalını aç; ilk parmağı modal açıkken kaldır; modalı kapatıp sürükle. | Giriş kilitli kalmaz; held/finger/dragging temizlenir ve yeni sürükleme yapılabilir. Mevcut Update/OpenModal akışında risk var; kod çıkarımı cihazda sınanır. | K / B04 |
| T012 | Gölet/nesne üst üste geldiğinde dokun; UI alanından dünyaya sürükle; dünya nesnesinden dışarı taşıyıp bırak. | Üst sıradaki etkileşim nesnesi seçilir; gölet arkada kalır. UI'dan başlayan dokunuş dünya işlemi başlatmaz. Pointer temizlenir. | K |

## B. Serbest bahçe

| ID | Senaryo / önkoşul ve uygulama adımları | Beklenen sonuç, gözlem ve geçme ölçütü | Önceki kapsam |
|---|---|---|---|
| T013 | Boş alan; Keşif aracıyla aynı alana iki kez bas; üçüncü kez bas. | plants 0→1→2; üçüncü dokunuş büyümeyi değil colors değerini değiştirir; keşif 0 ve plantedSpecies büyümede güncellenir. | E/W/A |
| T014 | Aynı deneyi Yağmur aracıyla yap; büyümüş alana tekrar su ver. | Büyüme min(2, eski+1); 2'de kalır, renk değişmez; yağmur/ses/geri bildirim görünür. Pul verilmez. | E/W |
| T015 | Güneşi boş alana, filize, açmış çiçeğe ayrı uygula. | Yalnız 1→2; 0 ve 2 değişmez. Açmış çiçekte “önce yağmur” mesajı mevcut UX tutarsızlığı olarak kaydedilir; ilerleme kaybolmaz. | E/W; B12 |
| T016 | Rüzgâr aracıyla boş alan, filiz, açmış çiçeğe dokun. | Mevcut Interact kuralıyla önce sulama, büyümüşte renk olur; alana dokunmak genel Wind çağrısı değildir. Bu davranış teknik referansla tutarlı olmalıdır. | K |
| T017 | Her türde 3 renk çevrimi yap, kapat/aç. | colors 0→1→2→0; tür kimliği sabit; renk/tür geri yüklenir. Seçilen alan diğer beş alanı değiştirmez. | W/A kısmi |
| T018 | Buluta 12 kez bas; bütün alanlar açtıktan sonra 3 kez daha bas. | İlk eksik alan indeks sırasıyla büyür; tamamlanınca rastgele alan seçilir; taşma olmaz. Bütün araçlarda bulut sulama yapar. | K |
| T019 | Bulutu alana 4 dünya biriminden yakın/uzak ve tam sınırda bırak. | <4 en yakın alanı sular; uzak/eşit durumda görsel yağmur ve dalga olur, büyüme değişmez. Piksel/dünya ölçüleri karıştırılmaz. | W/A kısmi |
| T020 | Cosmos/lavanta/gelincik, çan çiçeği/ayçiçeği, papatya/lale ve nilüferi büyütüp Keşif ile dokun. | Sırasıyla kelebek/Mina; alan indeksine bağlı nota; çiçek ölçeği/parçacık; dalga/Luli tepkileri. Her türün katalog behavior değeriyle eşleşir. | W yeni tür kısmi; K tümü |
| T021 | Serbest tekneye dokun; F0 kopyasında gölete <3, =3, >3 mesafeye sürükle. | Dokunma veya <3 bırakma boatLaunched ve keşif 3 yapar. Diğer bırakmalar başlatmaz; mini oyun teknesinin konumunu değiştirmez. | W/A kısmi |
| T022 | Tekne henüz açılmamışken Rüzgâr seçip tekne/gölete bas; 5 saniye bekle; tekrar bas. | Tekne otomatik açılır; keşif 5; WindUntil yaklaşık Time.time+5; yeniden basma süreyi yeniler. Normal hıza döner; pul verilmez. | W |
| T023 | Gölete Keşif/Yağmur/Güneş ve Rüzgârla ayrı dokun; tekne açıkken Luli'ye bas. | İlk üçünde dalga/Luli/notalar; rüzgârda Wind. Açık tekneyle Luli etkileşimi dalga üretir. | K |
| T024 | Elma/armudu pikniğe <2,6 ve dört başlangıç dostundan birine <1,5 uzaklıkta bırak; sınırları/uzak noktayı dene. | Geçerli bırakma ilk boş picnic hücresine 1/2 yazar, keşif 6; yanlış bırakma olumlu yönerge verir ama kayıt değişmez. Eşit sınır geçerli değildir. | E/W/A kısmi |
| T025 | Meyvelere ve sepete dokunarak dört yeri doldur; iki meyve daha ekle. | Meyve dokunuşu ilgili türü ekler; sepet elma/armut sırayla sunar. Doluysa yalnız picnic[0] yenilenir; döngüsel dört hücre değişimi beklenmez. | E; K UI |
| T026 | Dostları sürükle; x sınırı ±7,8 ve y -3,1/1,8 dışına bırak; yeniden başlat. | Bırakmada sınırlanır. Oturumda yeni merkezde kalır; yeniden başlatmada konumlar varsayılana döner; kalıcı dekorla karıştırılmaz. | W/A kısmi |
| T027 | İlk üç dekoru ve açılmış dekoru sürükle, yeniden başlat; sınır dışını dene. | Drop x ±7,8/y -3,7..1,4 sınırlar; JSON konumu korunur. Validate yüklemede y üst sınırı 1,8 olduğundan bu fark ayrıca kaydedilir. | E/W/A kısmi |
| T028 | 20 saniye işlem yapma; sonra ses çıkaran eylemleri hızlı tekrarla, dur. | Oyun ceza vermez; yaklaşık 14 saniyelik boşta yönerge, 5 saniyelik hint görünürlüğü; nota çağrıları 0,08 saniye ile sınırlanır. Eylem idle süresini yeniler. | K |

## C. Koleksiyon ve ekonomi

| ID | Senaryo / önkoşul ve uygulama adımları | Beklenen sonuç, gözlem ve geçme ölçütü | Önceki kapsam |
|---|---|---|---|
| T029 | F0, Bahçem'i aç; çiçek/dekor sekmeleri ve bütün sayfaları dolaş. | 8 çiçek, 12 dekor; sayfada en çok 4; başlangıçta 2 çiçek/3 dekor açık; etiketler katalog fiyatlarıyla aynı. | W/A kısmi |
| T030 | 0 pulla ücretli öğeye bas. | Bakiye/owned/placed değişmez; mevcut UI etkinlik seçim ekranına götürür. Kullanıcının nedenini anlayıp anlamadığı UX bulgusu olarak kaydedilir. | E; B14 |
| T031 | Tam fiyat kadar pul kazan; bluebell ve ayrı profilde 3 pulluk dekor al. | Bakiyeden tam fiyat bir kez düşer; kimlik unlocked'a bir kez eklenir. Yeni tohum keşfi 1 yalnız çiçek alımında oluşur. | E/W/A kısmi |
| T032 | Yetersiz, tam ve fazla bakiye ile katalogdaki her ücretli öğeyi dene. | Negatif bakiye yok; toplam 6 yeni çiçek 11, 9 yeni dekor 17 pul; hepsinin ilk alımı 28 pul. Yanlış kimlik/alınmış öğe doğrudan kuralda ücret kesmez. | E kısmi; K tüm katalog |
| T033 | Satın al düğmesine hızlı çift bas; uygulamayı kapatıp tekrar aç. | Tek sahiplik ve tek ücret; aynı öğeyi ikinci kez almak bakiyeyi değiştirmez. Sonradan oluşan UI aynı işlemi tekrarlamaz. | E/W kural; K gerçek çift dokunma |
| T034 | Yeni çiçeği satın al; dikim ekranını kapat; yeniden Bahçem'den seç. | Sahiplik kaybolmaz; dikim olmadan da satın alma kalıcıdır; ücretsiz yeniden dikilebilir. İptal satın alma iadesi değildir. | K |
| T035 | Her sahip olunan türü altı alanın her birine seçerek dik. | Yalnız seçilen flowerIds değişir; plants=1, colors=0 olur; eski tür koleksiyonda kalır. Türü bilmek/sahip olmak dışındaki alan kilidi yok. | W/A bir tür; K çapraz kapsam |
| T036 | Açmış çiçeğin üstüne aynı ve farklı tür dik; sonra büyüt. | Filize geri döner, renk sıfırlanır; yeni tür dikimi keşif 2; plantedSpecies ancak büyüdüğünde güncellenir. Onaysız değiştirme UX değerlendirmesine alınır. | W/A kısmi |
| T037 | Ücretli dekor al; kapat/aç; tekrar tekrar yerleştir/kaldır. | İlk satın alma ücretli, sonraki toggle ücretsiz; kaldırılınca dünyadan gizlenir, konumu saklanır; görünür yeni dekor keşif 8'i kalıcı açar. | E/W |
| T038 | Dekoru taşı, kaldır, yeniden yerleştir, uygulamayı yeniden aç. | Konum korunur; başlangıç dekorları da kaldırılabilir; sahip olunmayan dekor zorla görünür kalamaz. | E kural; K UI |
| T039 | Dekorların her birine dokun: pinwheel/chime/can/fountain/basket/birdhouse/lantern ve diğerleri. | Rüzgâr, sulama, dalga, elma paylaşımı, Luli tepkisi, fener parıltıları doğru indekslerde; diğerleri genel ses/parçacık tepkisi verir. Dekor kendiliğinden pul üretmez. | K |
| T040 | F2'de satın alma ve hemen sonraki yerleştirme kayıtlarını ayrı ayrı başarısız kıl. | İlk yazma başarısızsa para/sahiplik değişmez. İkinci yazma başarısızsa öğe sahipliği kalır; ücretsiz yeniden yerleştirilebilir. Hata görünür olmalı; sessiz kayıp kabul edilmez. | W yalnız readonly ilk işlem; B02/B13 |

## D. Mini oyunlar ve ödül

Ortak önkoşul: ilgili oyun daha önce tamamlanmamışsa variant=0. Sonraki düzenler ilgili activityPlays % 3 ile gelir. Her testte run id, activeActivity, activities[kind], stamps, activityCompletions ve activityPlays önce/sonra kaydedilir.

| ID | Senaryo / önkoşul ve uygulama adımları | Beklenen sonuç, gözlem ve geçme ölçütü | Önceki kapsam |
|---|---|---|---|
| T041 | Üç dünya etkinlik düğmesini tek tek aç; ilk hamle yapmadan çık ve tekrar gir. | Üç ayrı run oluşur; aynı tamamlanmamış türde id/düzen değişmez. activeActivity son açılan tür olur; oyunu açmak ödül vermez. | E/W/A kısmi |
| T042 | Piknik: doğru ilk meyve; yanlış ikinci meyve; sonra doğru dizi. Üç varyantı sırayla oyna. | Diziler [1,2,1], [2,2,1,1], [1,2,1,2,1]; yanlışta step/puan azalmaz, ilerlemez; doğru adım+1; 3/4/5 hedefte tamamlanır. | E bütün düzen; A/W ilk |
| T043 | Piknik yarıda “Birlikte yapalım”a bas; yardım ile bitir; manuel bitirişle karşılaştır. | Her yardım yalnız sıradaki doğru meyveyi ekler; ödül manuel oyunla aynı 1 pul; yardım sınırı/cezası yok. | E/A |
| T044 | Bulut'ta 3×3 hücrelerde rota dışına ve rota içine bas; her düzeni elle bitir. | Rota dışı değişmez; rota hücresi turns mod 4 döner; beş maskenin hedefle eşitliği tamamlanma şartı. Su görünümü bağlantılı ön eki izler; fizik simülasyonu değildir. | E tüm; W ilk elle |
| T045 | Bulut düz bir kanalı başlangıç hedefinden 180° döndür; dirseği 180° döndür. | Düz maskeler 5/10, 180°'de eşdeğerdir; dirsek eşdeğer değildir. Test yalnız turns==0 şartı aramaz; maskeyi karşılaştırır. | K |
| T046 | Bulut'ta yanlış yönlenmiş birkaç hücre oluştur; yardıma bir kez bas, sonra tekrarla. | Rota sırasındaki ilk yanlış hücre turns=0 olur; her yardım en çok bir hücreyi düzeltir. Tamamlanan düzende tekrar ödül verilmez. | E/A |
| T047 | Tekne'de 3 varyantın bütün duraklarına sırayla git; sonra sırayı bozmayı dene. | Başlangıçlar 10/0/10; duraklar [12,4]/[2,14]/[0,9,14]. Yalnız sıradaki durağa girince step artar; erken ziyaret sayılmaz. | E tüm yardım; W/A ilk |
| T048 | Tekneyi her kenardan dışarı ve kayaya doğru hareket ettir; geçerli başka yöne git. | Geçersiz yön ve engelde boatCell/step değişmez; yumuşak geri bildirim, can/süre kaybı yok; sonra geçerli hareket sürer. | E kısmi; K UI |
| T049 | Her tekne düzeninde başlangıçtan ve sapılmış her erişilebilir hücreden yardım kullan. | BFS sıradaki hedefe en kısa yolun tek adımını uygular; bütün geçerli konumlardan hedefe ulaşılır; kilitlenme olmaz. | E başlangıçlar; K tüm konumlar |
| T050 | Her türde ilk tamamlamayı yap; ödül ekranını aç/kapat; yeniden aynı tamamlanma çağrısını dene. | Tek id için claimed true ve toplam +1 pul/+1 completion/+1 ilgili plays; ikinci Claim ve yanlış id reddedilir. | E/W/A |
| T051 | Her türde dört tur tamamla; her yeni başlangıçtaki id ve variant'ı kaydet. | Varyantlar 0,1,2,0; yeni id; eski tamamlanmış run yerini yenisine bırakır. Sürekli artan zorluk, süre veya yeni dördüncü düzen yok. | E kısmi; K uçtan uca |
| T052 | Piknikte bir adım, Bulut'ta bir dönüş, Tekne'de bir hareket yap; türler arasında geçiş yap. | Üç ayrı yarım ilerleme korunur. Başka tür açmak önceki oyunu sıfırlamaz; bir oyunun ödülü diğerine yazılmaz. | W/A kısmi |
| T053 | Her türde yarıda arka plana al, süreç kapat, yeniden açıp aynı türü seç. | Aynı id/variant/adım/turns/boatCell geri gelir; ekran ana menüden açılır; ödül erken oluşmaz. | A piknik; K diğer |
| T054 | Son hamlenin kaydı başarılı, Claim kaydı başarısız olacak F2 oluştur; çık, düzelt, yeniden aç. | Tamamlanmış/claimed=false run korunur; yeniden açılışta Claim denenir ve yalnız 1 pul gelir. Hata görünür ve tekrar yolu anlaşılır olmalı. | E kural kısmi; B13 |
| T055 | Son hamle kaydını başarısız kıl; UI ve diski karşılaştır; hatayı kaldırıp aynı hamleyi yap. | Transaction geri döner: adım/turns/boatCell/ödül değişmez. Tekrar başarılı olduğunda bir kez ilerler. | K / B02 |
| T056 | Ödül ekranında “Bahçeye dön” ve “Bahçem” yollarını ayrı dene; aynı etkinliği yeniden aç. | Modal doğru kapanır/açılır; bakiye güncel; claimed run yerine yeni tur oluşturulur. Eski ödül yeniden verilmez. | W/A kısmi |
| T057 | Her mini oyunda çok hızlı giriş/yardım, modal kapama ve hemen başka tür açmayı tekrarla. | Geçersiz aktif run'a ödül/hamle uygulanmaz; kayıtlar tutarlı; hata/log istisnası veya takılma yok. | K |

## E. Keşifler ve Sumi

| ID | Senaryo / önkoşul ve uygulama adımları | Beklenen sonuç, gözlem ve geçme ölçütü | Önceki kapsam |
|---|---|---|---|
| T058 | Dört projenin 12 keşfini teknik referanstaki tetiklerle birer birer aç. | Her eylem doğru flag'i açar; kart ilerlemesi 0–3; ilk üç projenin görsel parçaları ilerleme sayısına göre belirir. Pul ayrı mini oyun sisteminden gelir. | E/W tümü toplu; K tek tek |
| T059 | Keşifleri kart sırasını takip etmeden yap; her biri için tekrar eylem uygula. | Sıra zorunlu değildir; true bayrak tekrar artmaz. Proje/keşif için ikinci ödül veya zorunlu bölüm geçişi yok. | K |
| T060 | Cosmos ve daisy büyüt; yeni türü yalnız dik; sonra büyüt. | İlk aşamada species=2; dikmek sayacı artırmaz; üçüncü tür açınca keşif 10 olur. UI'daki “dik” metni gerçek kurala uygun hâle getirilmeden UX kabulü geçmez. | W / B12 |
| T061 | Sumi projesinin üç şartını tamamla, diğer projelerden en az birini eksik bırak. | Sumi görünür; bütün dört projeyi tamamlama zorunluluğu yok. Diğer projelerin durumu değişmez. | E kural; K UI |
| T062 | Sumi açıkken dokun, hareket ederken tekrar dokun, sürükle; meyve bırak; uygulamayı yeniden aç. | Yürüyüş tekrar yığılmaz; sürükleme keser; konum oturumluktur. Meyve diğer dostlardaki gibi kabul edilmeli: mevcut i<4 koşulu bunu dışladığı için açık hata kaydedilir. | K / B09 |
| T063 | Her kartta ilk eksik adıma yönlendir; sonra tamamlanmış kartın “Tamamlandı” düğmesine bas. | Eksik adım uygun ekran/ipucu açar. Tamamlanmış kart başka projenin görevine yönlendirmemeli; mevcut taşma kabulü geçmez. | K / B06 |
| T064 | 12 keşif tamam ve bütün içerik açıkken 5 tur daha oyna; albüm kullan. | Oyun serbestçe sürer; kredi/son ekran/yeniden başlangıç zorunluluğu yok. Pullar kazanılır; sonsuz yeni içerik veya seviye vaadi gösterilmez. | K |

## F. Ayarlar, yetişkin alanı, erişilebilirlik

| ID | Senaryo / önkoşul ve uygulama adımları | Beklenen sonuç, gözlem ve geçme ölçütü | Önceki kapsam |
|---|---|---|---|
| T065 | Ana ekran ve bahçede dili değiştir; her modalı TR/EN aç; yeniden başlat. | Bütün metinler doğru dilde, taşmadan; ayar kalıcı. Açık yetişkin ayarları Rebuild sonrası ayar ekranına döner. | W/A kısmi |
| T066 | Ses kapat/aç: ambiyans, yağmur, nota, UI düğmesi ve mini oyun ödülünde dene; yeniden başlat. | Kapalıyken bütün oyun AudioSource'ları susar; kalıcıdır; geri açılınca beklenen sesler döner. UI click'in değiştirme anındaki sırası ayrıca kaydedilir. | A mute; K tümü |
| T067 | Yetişkin kapısında yanlış yanıt, sonra doğru yanıt; X/Geri ile kapat; yeniden aç. | Yanlışta yeni soru, doğruysa ayarlar; kapama dünya kontrolünü geri verir. Bu kapı kimlik/yaş doğrulaması veya ebeveyn rızası değildir. | A eski test |
| T068 | Azaltılmış hareket açık/kapalı; yağmur, rüzgâr, dost tepkisi, kelebek, tekne ve Sumi'yi karşılaştır. | Ayar kalıcı; bütün etkiler tek tek gözlenir. Beklenen erişilebilirlik kapsamı belirlenmeli; mevcut yağmur/rüzgâr ve bazı scale efektleri devam ettiği için “hareket tamamen kapalı” diye kabul verilmez. | A ayar kaydı; B10 |
| T069 | Ayarlar içindeki bilgi/gizlilik metnini her dilde aç; geri dön. | Metin okunur; gerçek veri/SDK/pul alışverişi davranışını doğru açıklar. Yayıncı ve gizlilik adresi placeholder kalıyorsa release kabulü verilmez. | A görünüm; B12/B25 |
| T070 | F1'de sıfırlama iste; iptal et; sonra tekrar iste ve onayla; yeniden başlat. | İptalde değişiklik yok. Onayda ilerleme/bakiye/keşifler başlangıç; dil/ses/hareket korunur, visited true. Albüm ve yedek kapsamı açıkça belirtilir; “bütün veriler silindi” yanlış beyanı yok. | A eski iptal; B07 |
| T071 | Rüzgâr açık, farklı araç seçili, Sumi yürüyor, dostlar taşınmışken reset; ayrıca reset yazmasını başarısız kıl. | Başarısız reset eski ilerlemeyi korur ve görünür hata verir. Başarılı reset sonrası seçili araç/animasyon/konum davranışı belirlenen başlangıçla tutarlı olmalı; mevcut oturum kalıntıları B07'ye işlenir. | K / B02/B07 |

## G. Kayıt, göç ve bozuk veri

Kayıt testleri için uygulama kapalıyken F2 hazırlamak esastır. Kural beklentisi ile kullanıcıya görünür kabul sonucu ayrı ayrı gözlenir.

| ID | Senaryo / önkoşul ve uygulama adımları | Beklenen sonuç, gözlem ve geçme ölçütü | Önceki kapsam |
|---|---|---|---|
| T072 | Normal kayıtla büyüme, renk, tür, konum, piknik, tekne, bütün ayarları değiştir; kapat/aç. | Kalıcı sözlükteki alanlar eşit; oturumluk araç/rüzgâr/karakter konumu otomatik kalıcı sayılmaz. JSON version 2 olur. | E/W/A kısmi |
| T073 | Bir normal Write öncesi dosyayı sakla; yaz; ana/.bak karşılaştır. | Ana yeni durumu, .bak önceki geçerli durumu taşır; .tmp başarıdan sonra kalmaz. Yedek en son eylemi değil bir önceki sürümü tutabilir. | E kısmi |
| T074 | F2 ana dosyayı kes/boz, geçerli .bak bırak; başlat ve kaydet. | Yedek yüklenir; Recovered true; kayıt tekrar geçerli olur. Kurtarmanın oyuncuya bildirimi ayrıca incelenir; şu an özel UI yok. | E |
| T075 | Ana ve yedeği ayrı ayrı bozuk, boş ve geçersiz JSON yap; başlat. | Çökmeden yeni profil; log hata bilgisi; eski dosyaların üzerine yazma/kurtarma davranışı açıkça raporlanır. Sessiz ilerleme kaybı için B03 değerlendirilir. | E kısmi / B03 |
| T076 | Gerçek v1 örneğini F2'ye koy; yükselt; hash ve alanları karşılaştır. | Eski üç alan korunur; renk 1 papatyaya, diğerleri cosmos'a göçer; tint sıfır; yeni alanlar varsayılan. Özgün .v1-preserved byte içeriği değişmez; şema 2 olur. | E |
| T077 | Geçerli v1'i okunabilir yap fakat .v1-preserved oluşturmayı engelle; başlat. | Geçerli ilerleme yüklenmeli ve koruma hatası açıklanmalı; mevcut catch'in yedeğe/yeni profile düşmesi kabul hatasıdır. | K / B08 |
| T078 | F2'de version=99 ana dosya; eski geçerli .bak; aç, etkileşim/satın alma/reset dene. | Yeni sürüm dosyası hiçbir yazmada değişmez; ReadOnly. Oyuncu kayıt korunuyor ve bu sürümle ilerleyemiyor bilgisini görmeli; geçici boş bahçenin sessiz gösterimi kabul edilmez. | E/W kural; B03 |
| T079 | Dizileri null/kısa/uzun, dil geçersiz, büyümeyi -1/3, rengi -1/4 yap; yükle. | Validate boyutları 6/4/12/3'e normalleştirir; büyüme/renk 0..2; dil tr; exception yok; düzelen alanlar log/fixture karşılaştırmasıyla doğrulanır. | E kısmi |
| T080 | Bilinmeyen/tekrarlı/unowned türler, negatif pul, aşırı pul, görünür kilitli dekor ve yinelenen species hazırla. | Bilinmeyenler elenir; starter sahipliği geri gelir; geçersiz/unowned ekili tür cosmos; pul 0..1.000.000; kilitli dekor gizli; species tekilleşir. | E kısmi |
| T081 | Konumlarda sınır dışı, NaN, Infinity; yanlış kind/variant/turns/id taşıyan activity kayıtları hazırla. | Konumlar sınırlanır/geçersizler varsayılan; yapısal olarak geçersiz run temizlenir; diğer türlerin sağlam run'ları korunur. | E kısmi |
| T082 | Yapısal Valid geçen fakat anlamca bozuk run: claimed=true eksik, step hedef üstü, tekne kaya üstünde, tutarsız sayaçlar. | Kilitlenme/haksız ödül üretilmemeli; onarım ya da reddetme gerekir. Mevcut Valid yeterli değil; beklenen onarım henüz uygulanmadı. | K / B15 |
| T083 | F2'de normal büyüme/ayar yazmasını ve ayrı testte Transaction satın alma/hamle yazmasını engelle. | Transaction belleği değiştirmez. Doğrudan Save yolunda bellek ilerlemesi geri alınmıyor: kullanıcıya kaydedilmediği görünür olmalı; başarı mesajı hatayı örtmemeli. | W readonly işlem; B02 |
| T084 | File.Replace desteklenmeyen ortamda fallback sırasında işlem kesintisi; ayrıca .bak kopyası/temp yazması hatası enjekte et. | Son geçerli kayıt veya sağlam yedekle kurtarma; yarım ana dosya sessiz kabul edilmez. Fallback atomik değil; güç kesintisi güveni test olmadan verilmez. | K / B05 |
| T085 | Yalnız sağlam .tmp bırak, ana/yedek yok; ayrı testte v1-preserved var ana/yedek yok. | Mevcut loader bunları okumaz, yeni profil açar. Release kurtarma politikası bu durumu açıkça ele almalı; otomatik kurtarıldı diye raporlanmaz. | K / B05/B07 |
| T086 | F2'de stamps 999.999/1.000.000; activity sayaçlarını int sınırına getirip ödül dene. | Bakiye sınırı ve sayaç taşması ödül metniyle tutarlı olmalı; negatif/yeniden sıfırlanmış sayaç ve haksız varyant olmaz. Mevcut clamp/unchecked increment riski açık. | K / B15 |
| T087 | İşletim sistemi yedekleme/geri yükleme, uygulama verisi temizleme, kaldırıp kurma, paket kimliği ve imza değişimi akışlarını ayrılmış cihazda dene. | Hangi veri korunuyor/kayboluyor kaydedilir; vaatler gerçek davranışa göre yazılır. OS backup kapalı varsayılmaz; uygulama içi bulut hesap özelliği var sayılmaz. | K / B21 |

## H. Anılar / yerel albüm

| ID | Senaryo / önkoşul ve uygulama adımları | Beklenen sonuç, gözlem ve geçme ölçütü | Önceki kapsam |
|---|---|---|---|
| T088 | F0, Anılar; yeni anı çek; albümü yeniden aç. | Boş durum anlaşılır; garden-album içinde PNG; kamera/haricî depolama izni sorulmaz. Oyun ekranı/HUD görüntüsü gösterilir. | W/A tek PNG |
| T089 | Art arda farklı 7 anı oluştur; dosya adları ve albüm oklarını kontrol et. | En fazla 6 PNG; normal ileri saatte en eski silinir, en yeni korunur; önce/sonra gezinme başa sarar. | K |
| T090 | Anıdan sonra kapat/aç; ileri/geri gezin; bahçeyi resetle. | Normal yeniden başlatmada albüm kalır; reset şu anda albümü silmez. UI reset kapsamını doğru anlatmalı; silme özelliği varmış gibi davranılmaz. | K / B07 |
| T091 | Albüme bozuk, sıfır bayt ve PNG olmayan .png ekle; gez. | Çökmeden okunamadı bilgisi ve diğer resimlere geçiş. LoadImage false sonucu sessiz boş resim kabul edilmemeli. | K / B11 |
| T092 | Albüm klasörüne erişimi ve PNG yazmasını kontrollü engelle; Anılar/Yeni anı aç. | Görünür hata, çalışır UI; istisna akışı takmaz. GetFiles'ın try dışında olması özellikle sınanır. | K / B11 |
| T093 | Yakalama anında uygulamayı arka plana al/ekranı değiştir; texture null senaryosu enjekte et. | Başarı olmayan işlem başarı diye sunulmaz; eski albüm sağlam; tekrar çekim mümkün. | K / B11 |
| T094 | Cihaz saatini geriye alıp yeni anı çek; çok hızlı iki çekim iste. | Yeni anı yanlışlıkla silinmez/üzerine yazılmaz; dosya adı çakışması ve tarih sırası yönetilir. Mevcut isim-sırası budaması risklidir. | K / B11 |
| T095 | En yüksek hedef çözünürlükte 20 kez albüm aç/kapat ve çekim yap; bellek/kare süresini ölç. | Sürekli büyüyen texture/bellek veya kabul dışı ana iş parçacığı duraklaması yok. Tek texture'ın modal kapandıktan sonra tutulması ölçülür. | K / B11/B17 |
| T096 | Albüm görüntülerinde kişi bilgisi/cihaz arayüzü bulunup bulunmadığını; oyundan dışarı aktarım yolunu kontrol et. | Yalnız oyun framebuffer'ı; mevcut UI'da paylaşım, yükleme, galeri veya silme düğmesi yok. Gizlilik metni gerçek yerel dosya kapsamıyla uyumlu. | K |

## I. Yaşam döngüsü, platform ve yayın adayı

| ID | Senaryo / önkoşul ve uygulama adımları | Beklenen sonuç, gözlem ve geçme ölçütü | Önceki kapsam |
|---|---|---|---|
| T097 | Dünya sürüklemesi sırasında bildirim, Home, ekran kilidi, odak kaybı; geri dön. | Pointer iptal, nesne geçerli merkeze döner; kayıt denenir; yeni parmak kabul edilir; mini oyun bozulmaz. | A pause; K drag |
| T098 | Mini oyunun son hamlesi/ödülü/satın alma yakınında süreci farklı noktalarda öldürüp aç. | Diskte son tamamlanan işlem geri gelir; fazla ücret/çift ödül yok; tamamlanmış-ödülsüz run tekrar alınabilir. İşlem ortası güç kesintisi ayrı T084'tür. | A restart kısmi |
| T099 | 16:9, 4:3, uzun ekran, tablet/çentik; her modal açıkken yatay yönü çevir veya pencereyi yeniden boyutlandır. | Safe area/ölçek doğru; modal üstte ve bütün düğmeler erişilebilir; dünya etkinlik düğmeleri nesnelerle hizalı. Rebuild sıralaması B16 test edilir. | W iki sabit oran; K dinamik |
| T100 | Bütün metin/düğme ve dünya hedeflerini hedeflenen küçük fiziksel ekranda çocuğun erişimi açısından gözle. | Okunabilir metin; ölçülmüş dp hedef boyutları; seçilemeyen/yanlışlıkla tetiklenen hedef yok. 1080 referansındaki birim doğrudan fiziksel dp kabul edilmez. | K / B24 |
| T101 | Temiz kurulumdan itibaren Wi-Fi/veri kapalı 30 dakika; bütün modlar, albüm, yeniden başlat. | Bağlantı bekleme/hesap isteği yok; bütün yerel mekanikler çalışır. Gerçek build ağ/izin incelemesiyle tutarlı davranır. | A temel offline; K 30 dk |
| T102 | Fiziksel düşük/orta ARM64 telefon, güncel telefon ve tablette ilk açılış ve 30 dakikalık karışık oturum ölç. | Çökme/ANR yok; minimum 30 FPS hedefi sağlanır, 60 tercih edilir; ısı/bellek/kare süresi kaydedilir. RAM/başlangıç süresi sayısal eşikleri ilk ölçümden sonra release kapısında belirlenir. | K / B17/B23 |
| T103 | API26 alt sınır ve API36 üzerinde kurulum/başlatma; uyumsuz eski cihazda yükleme davranışı. | Desteklenenlerde aynı temel akış; min SDK altına destek vaadi yok. Sadece manifest değeri API26 çalıştığı kanıtı sayılmaz. | P manifest; A API36 |
| T104 | 16 KB sayfalı ARM64 cihaz/uygun test ortamı; gerçek sayfa boyutunu doğrula; cold start ve bütün modlar. | Native yükleme hatası yok; kayıt/albüm/yaşam döngüsü çalışır. ZIP/ELF hizalama sonucu runtime testinin yerine geçmez. | P statik; K runtime |
| T105 | Windows normal oynanış ve iki smoke oranında kapat; exit code/crash log kontrol et. | İşlev raporu geçti **ve** süreç temiz kapandı; mevcut -1073741819 ile KALDI. Android sonuçları bu masaüstü hatasını kapatmaz. | W başarısız / B01 |
| T106 | Final Android adayının manifest, bütün izinleri, ABI, SDK, debuggable, native hizalama ve sertifikasını bağımsız denetle. | Nihai kimlik/sürüm ve production imzası doğru; yasak/beklenmeyen izin yok; ARM64/API36 ve 16 KB dosya kontrolleri geçer. Script'in sabit signing yazısı kanıt sayılmaz. | P kısmi / B19 |
| T107 | Final AAB'yi bundletool/Play internal kanalından kur; split APK ile mini oyun/albüm/ses/göç akışlarını dene. | Aynı işlevler, uygun kaynak/native kitaplıklar, doğru imza; test APK sonucu AAB sonucu yerine yazılmaz. | K / B18 |
| T108 | Aynı release imzası/paketle eski test profilinden yeni versionCode'a güncelle; veri ve albümü karşılaştır. | Yükseltmede veri korunur; v1→v2 testleri geçer; farklı imza varsa beklenen kurulum engeli açıkça tanınır. Uninstall veri koruma yöntemi olarak kullanılmaz. | E göç; K gerçek update |
| T109 | Temiz ASCII staging ile build; kaynak dosya silinmiş eski staging ile karşılaştır; raporların zaman/hash'ini denetle. | Kaynak manifesti ve build içeriği eşleşir; önceki koşunun sonuçları yeni test diye sunulmaz. Başarısız/timeout test geçerli yeni rapor üretmiyorsa test KALDI/ENGELLİ. | K / B20/B22 |
| T110 | Hedef yaşa uygun gözlem oturumunda ana ekran→mini oyun→ödül→dikim ve yardım akışını yetişkin gözetimiyle dene. | Çocuk hangi eylemi yapacağını, kazandığı pulu ve yeni çiçeği anlayabilir; korkutucu/cezalandırıcı geri bildirim yok. Gözlemler ve yardım ihtiyacı kaydedilir; önceden test edilmiş sayılmaz. | K / B24 |
| T111 | Mağaza bilgisi, hedef yaş, içerik derecelendirmesi, gizlilik adresi, Data Safety ve ekran görüntülerini final binary ile karşılaştır. | Planlanmış sosyal/günlük/online özellik vaat edilmez; gerçek veri/SDK/ödül davranışı doğru; zorunlu alanlar dolu ve destek adresi işler. | K / B25/B26 |
| T112 | İç/kapalı dağıtım sonrasında temiz kurulum ve güncelleme; crash/ANR ve destek sinyallerini izle; düzeltme sürümü denemesi yap. | Yayın kanalındaki hash/sürüm test edilen adayla ilişkili; kritik regresyon yok; dağıtımı durdurma ve daha yüksek versionCode ile düzeltme süreci belgeli. | K / B26 |

## Kapsamın kullanılması ve çıkış ölçütleri

Teknik referanstaki aksiyon gruplarıyla temel izlenebilirlik:

| Aksiyonlar | Temel testler | Kesinti/bozuk durum ekleri |
|---|---|---|
| N01–N05, N08–N10 | T001–T012, T067 | T097, T099 |
| N06–N07, N11–N17 | T065–T071, T097–T098, T105 | T078, T083, T087 |
| W01–W21 | T009–T028, T039, T062 | T011, T068, T071, T083, T097 |
| C01–C08 | T029–T040 | T078–T083, T098 |
| C09–C10 | T058–T064 | T060, T063, T082 |
| M01–M09 | T041–T057 | T078–T086, T098 |
| H01–H03 | T088–T096 | T070, T087, T095 |

- **Hızlı regresyon:** T001, T003, T006, T009, T013, T021, T024, T031, T035, T037, T042, T044, T047, T050, T052, T053, T058, T065, T066, T070, T072, T088, T097, T101, T106. Her değiştirilmiş alanın sınır testi ayrıca eklenir.
- **Tam aday kabulü:** ilgili platformdaki bütün satırlar uygulanır; saf kurallardaki üç varyant + gerçek UI'da üç varyant ayrıdır. Kapsam dışı/platform dışı satır için gerekçe yazılır; otomatik “geçti” verilmez.
- **Veri/ödül önceliği:** T040, T050, T054–T055, T071, T073–T087 ve T098. Bu alanlarda kayıp, haksız ücret, çift ödül veya kalıcı kilit varsa aday durur.
- **Görsel/cihaz kabulü:** sabit ekran görüntüsü onayı, dokunma ve dinamik modal testinin yerine geçmez. Android release için fiziksel ARM64 ve 16 KB çalışma açığı kapatılmalıdır.
- **Performans:** depodaki mevcut ürün hedefi en az 30 FPS, tercih 60. Başlangıç süresi, bellek tepe değeri, ısı ve uzun oturum için henüz ölçülmüş kabul bütçesi yok; ilk cihaz ölçümü sonrası yazılmalıdır. Uydurma eşik/ölçümle kabul verilmez.
- **Yayın kararı:** açık P1 sorun ve başarısız kritik test yok; bilinen P2'ler kapsam/etki ve gerekçeyle kaydedilmiş; mağaza ve kimlik bağımlılıkları tamamlanmış olmalı. Windows kapanış hatası Android adayından ayrı izlenir; Windows testini geçmiş gibi göstermeye izin vermez.

Sonuç CSV'sinde bir satır bir temel senaryodur. Cihaz, varyant veya hata türüne göre tekrar gerekiyorsa aynı ID'yi alt koşul etiketiyle çoğalt; toplam 112 sayısı bütün kombinasyonların sayısı değildir.
