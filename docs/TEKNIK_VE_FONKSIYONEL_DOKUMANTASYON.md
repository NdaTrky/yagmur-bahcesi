# Yağmur Bahçesi — teknik ve fonksiyonel referans

İnceleme: **3 Kasım 2026, Europe/Istanbul**. Ürün: **0.2.0 / Android versionCode 2**. İncelenen oyun kodu: **0d2a5c993c9793cd5f5623df2dc3719ab488e7a3**. Bu çalışma dokümantasyon ve kod incelemesidir; yeni özellik, hata düzeltmesi veya yayın yapıldığı iddiası taşımaz.

Bu belge önceki fikir belgelerinden değil, aşağıdaki kaynaklardan çıkarılmıştır. Önceki GDD 0.1 tasarım kaydıdır; öneri belgeleri uygulanmış davranışın kanıtı değildir.

**Okuma ve çalışma sırası**

1. Bu belge: mimari, veri, ekranlar, bütün kullanıcı aksiyonları ve algoritmalar.
2. [Test senaryoları ve kabul kriterleri](TEST_SENARYOLARI_VE_KABUL_KRITERLERI.md): adımlar, beklenen sonuç, gözlenecek alanlar ve kanıt kapsamı.
3. [Kod denetimi ve açık konular](KOD_DENETIMI_VE_ACIK_KONULAR.md): uygulanmış/test edilmemiş/eksik/öneri ayrımı ve bulgular.
4. [Release çalıştırma planı](RELEASE_CALISTIRMA_PLANI.md): Android adayı oluşturma, doğrulama, mağaza ve yayın sonrası adımlar.

## Kanıt dili ve kaynak haritası

- **D — doğrulanmış:** depoda ilgili davranışı kapsayan geçen test çıktısı var. Belirtilen ortam ve senaryo için geçerlidir.
- **K — kodda mevcut:** uygulama yolu okunmuş; ilgili davranışın ayrı çalışma testi kanıtı yok veya yalnız bir alt örneği var.
- **A — açık/eksik:** bilinen başarısız test, uygulama kusuru veya tamamlanmamış gereksinim.
- **Ö — öneri:** henüz oyun kodunda bulunmuyor.
- **Koddan çıkarım:** akıştan elde edilen bulgu; cihazda tekrar edilmiş hata olarak sunulmaz.

| Kısaltma | Kaynak ve sorumluluğu |
|---|---|
| G | [GardenGame.cs](../YagmurBahcesi/Assets/RainGarden/Scripts/GardenGame.cs): Awake, Interact, büyüme, giriş, Save, ResetGarden, yaşam döngüsü |
| P | [GardenProgression.cs](../YagmurBahcesi/Assets/RainGarden/Scripts/GardenProgression.cs): Transaction, katalog işlemleri, mini oyun entegrasyonu, projeler, Sumi, albüm |
| S | [GardenState.cs](../YagmurBahcesi/Assets/RainGarden/Scripts/GardenState.cs): GardenState, GardenRules, GardenSave |
| A | [GardenActivities.cs](../YagmurBahcesi/Assets/RainGarden/Scripts/GardenActivities.cs): durum, üç kural takımı, yardım, Claim, GardenProjects |
| C | [GardenCatalog.cs](../YagmurBahcesi/Assets/RainGarden/Scripts/GardenCatalog.cs): tür kimlikleri, fiyatlar, koordinatlar, sahiplik |
| U | [GardenUI.cs](../YagmurBahcesi/Assets/RainGarden/Scripts/GardenUI.cs): ana ekran, araçlar, yetişkin kapısı, ayarlar, modal altyapısı |
| X | [GardenExpansionUI.cs](../YagmurBahcesi/Assets/RainGarden/Scripts/GardenExpansionUI.cs): koleksiyon, dikim, keşif, etkinlik ve albüm ekranları |
| E | [GardenEntity.cs](../YagmurBahcesi/Assets/RainGarden/Scripts/GardenEntity.cs): karakter/nesne hareketi ve parçacıklar |
| B | [GardenBuilder.cs](../YagmurBahcesi/Assets/RainGarden/Editor/GardenBuilder.cs): Editor üzerinden sahne, görsel/ses, ayar ve build |
| T | [GardenChecks.cs](../YagmurBahcesi/Assets/RainGarden/Editor/GardenChecks.cs), [GardenSmoke.cs](../YagmurBahcesi/Assets/RainGarden/Scripts/GardenSmoke.cs): özel test koşucuları |

İşlev adları esas referanstır. Bu sürümde önemli başlangıç satırları: G.Awake 36, G.Interact 85, G.PointerDown 217, G.Drop 251; P.Transaction 13, P.ActivityInput 97; S.Validate 30, S.Load 85, S.Write 102; A.Begin 21, A.Help 66, A.Claim 87; X.GuideProject 106, X.ShowActivityReward 200. Kod değişince satırlar yeniden kontrol edilmelidir.

## 1. Teknik altyapı ve kullanılan teknolojiler

### 1.1 Teknoloji envanteri

| Katman | Gerçek uygulama |
|---|---|
| Motor | Unity **6000.3.17f1**, [ProjectVersion.txt](../YagmurBahcesi/ProjectSettings/ProjectVersion.txt) |
| Oyun dili | C#; UnityEngine MonoBehaviour, coroutine, ScriptableObject ve System.IO/LINQ/Collections |
| Grafik | Yerleşik render pipeline; ortografik kamera, SpriteRenderer ve PNG resimler. GraphicsSettings içinde özel pipeline yok |
| Arayüz | **com.unity.ugui 2.0.0**; Canvas, CanvasScaler, Image, Text, Button, GraphicRaycaster. UI çalışma sırasında C# ile kurulur |
| Giriş | Eski Unity Input API; fare ve dokunma; EventSystem + StandaloneInputModule. Yeni Input System paketi kullanılmıyor |
| Fizik | Physics2D.OverlapPointAll ve BoxCollider2D seçim alanları; yerçekimi sıfır. Oyun kodunda Rigidbody2D/sıvı simülasyonu yok |
| Animasyon | Transform/sprite değişimi, sinüs fonksiyonları, coroutine ve süreli parçacıklar. Animator/rig tabanlı karakter sistemi yok |
| Ses | AudioSource; 5 nota, yağmur, ambiyans. WAV'lar Editor koduyla 22.050 Hz, mono, 16 bit PCM üretilir; konuşmalı yönerge yok |
| Font/sanat | Nunito (SIL OFL); proje için üretilmiş PNG atlasları ve ikon. Görsel üretim geliştirme aşamasıdır; oyun içinde üretken AI çağrısı yok |
| Veri | Unity JsonUtility ile yerel JSON; PNG albüm dosyaları. PlayerPrefs/SQL veritabanı kullanılmıyor |
| Android derleme | IL2CPP; ARM64 telefon APK, X86_64 development emülatör APK. SDK Build Tools 36.0.0, NDK 27.2.12479018, OpenJDK 17.0.18 yerelde incelendi |
| Windows önizleme | Mono tabanlı Windows Player; mevcut BuildWindows, BuildOptions.None kullanır. Çalıştırıcı D3D11 seçer |
| Otomasyon | PowerShell, Unity -batchmode/-executeMethod, ADB, aapt2, zipalign, llvm-readelf; Git ile kaynak sürümleme |
| Test altyapısı | Özel C# assertion koşucuları; NUnit/Unity Test Framework paketi veya bulut CI hattı yok |

Paketlerin tam listesi [manifest.json](../YagmurBahcesi/Packages/manifest.json) ve [packages-lock.json](../YagmurBahcesi/Packages/packages-lock.json) içindedir. Sürüm 1.0.0 olan yerleşik modüller: screencapture, animation, imageconversion, jsonserialize, physics2d, ui, imgui, audio. Bir modülün pakette bulunması bütün API'lerinin oyunda kullanıldığını göstermez.

B.Settings: yatay sol/sağ dönüş açık, dikey kapalı; hedef kare hızı G.Awake'te 60; vSync/AA/gölge kapalı; Gamma renk uzayı; Android min API 26, target API 36; managed stripping Low, engine stripping açık. 60 değeri ölçülmüş FPS değildir.

### 1.2 Frontend, backend, veritabanı ve API

Oyuncunun gördüğü katman Unity sahnesi ve UGUI'dir; HTML/React/web frontend yoktur. Oyun kuralları aynı cihazdaki C# kodunda çalışır. Ayrı sunucu, hesap servisi, HTTP API, merkezi veritabanı, giriş/token, bulut eşitleme, uzaktan içerik, ödeme SDK'sı veya çevrimiçi oyuncu sistemi uygulanmamıştır.

Frontend/backend ayrımı burada **görünüm → yerel denetleyici → yerel durum/kural → dosya** ayrımıdır. P.Transaction bir sunucu ya da veritabanı transaction'ı değildir. Para/ödül hesabının otoritesi kullanıcının cihazıdır.

~~~mermaid
flowchart LR
    Input["Dokunma / fare"] --> UI["UGUI ve dünya seçimi"]
    UI --> Game["GardenGame + GardenProgression"]
    Game --> Rules["GardenRules / Activities / Catalog / Projects"]
    Rules --> State["Bellekte GardenState"]
    State --> Save["GardenSave ve JsonUtility"]
    Save --> Disk["JSON + yedekler"]
    State --> Render["Sprite, UI, ses ve hareket"]
    UI --> Album["Ekran görüntüsü"]
    Album --> PNG["Yerel PNG albümü"]
~~~

### 1.3 Sahne ve varlık yaşam döngüsü

Tek build sahnesi **Assets/RainGarden/Scenes/Garden.unity**. B.Setup sahne yoksa oluşturur; varsa görsel referanslarını tazeler. B.Settings her çalıştırmada ürün/platform ayarlarını tekrar yazar.

Kaynak sahnede 16 GardenEntity vardır: 3 yatak, 4 dost, 1 bulut, 1 tekne, 2 meyve, 3 dekor, 1 gölet alanı, 1 piknik alanı. P.ExpandWorld çalışma sırasında 3 yatak, 9 dekor ve Sumi ekler: toplam **29 kayıtlı etkileşim nesnesi**. Kilitli/yerleştirilmemiş olanlar etkin değildir. Buna ek olarak etkileşim bileşeni olmayan 3 saksı resmi, 6 kelebek, 9 proje parçası ve geçici efektler vardır. Arka plandaki her çizim seçilebilir değildir.

GardenArt ScriptableObject, mevcut dört dostun 16 pozunu, 12 temel nesneyi, 16 genişleme sprite'ını ve Sumi'nin 4 pozunu tutar. Runtime karakter kodu normal/sevinç/göz kırpma pozlarını kullanır; dördüncü pozun atlas içinde bulunması ayrı bir oynanış durumu değildir.

[GardenCamera.LateUpdate](../YagmurBahcesi/Assets/RainGarden/Scripts/GardenCamera.cs) ortografik yüksekliği max(5,4; 9,6 / max(0,5; aspect)) yapar; dünya oranlara göre görünür kalır. UI 1920×1080 referanslı ScreenSpaceOverlay Canvas kullanır, yükseklik eşleşmesi 1'dir ve Screen.safeArea içine yerleşir. Modal paneli ayrıca kullanılabilir genişliğe göre küçültülür. Dünya düğmeleri WorldToScreenPoint ile UI konumuna çevrilir; açık modal sırasında boyut değişimi ayrı test gerektirir.

### 1.4 Kalıcı veri sözlüğü

Kaynak: S.GardenState. **Bir yerel profil** bulunur. Ekrandaki 1–6 numaralı dikim alanları kodda 0–5 indeksidir.

| Alan | Tür / başlangıç | Anlam |
|---|---|---|
| version | int = 2 | JSON şema sürümü; dosya adındaki v1 ile karıştırılmamalı |
| plants | int[6], hepsi 0 | 0 başlangıç, 1 filiz, 2 açmış çiçek |
| colors | int[6], hepsi 0 | 0 özgün, 1 maviye, 2 pembeye yakın çarpan; çiçek türü değildir |
| flowerIds | string[6] | cosmos, cosmos, cosmos, daisy, cosmos, daisy |
| boatLaunched | bool = false | Serbest bahçe teknesi gölete bırakılmış mı |
| picnic | int[4], 0 | 0 boş, 1 elma, 2 armut; mini oyundan ayrıdır |
| decorations | Vector2[12] | Her dekorun konumu; C.DecorPositions başlangıcı |
| placed | bool[12] | İlk üç true, diğerleri false |
| unlocked | string[] | cosmos, daisy, pinwheel, stones, mushroom başlangıçta açık |
| stamps | int = 0 | Harcanabilir pul; Validate aralığı 0–1.000.000 |
| discoveries | bool[12], false | Proje keşifleri; tetiklendikten sonra normal oyunda geri alınmaz |
| plantedSpecies | string[], boş | En az bir kez **tam büyümüş** çiçek türleri; adı dikimden geniş anlam taşıyor |
| activityCompletions | int = 0 | Ödülü kaydedilmiş toplam etkinlik sayısı; XP değildir |
| activityPlays | int[3], 0 | Her mini oyunun kaydedilmiş tamamlama sayısı; sonraki düzeni seçer |
| activities | GardenActivityState[3] | Her türün son/yarım etkinliği; eski bütün oyunların geçmişi tutulmaz |
| activeActivity | int = 0 | O an kullanılan etkinlik dizini; ekrana otomatik dönme garantisi vermez |
| activity | hesaplanan property | activities[activeActivity]; ayrı JSON alanı değildir |
| language | tr veya en | Yeni gerçek kayıtta cihaz Türkçe ise tr, diğer dillerde en |
| sound | bool = true | Nota/yağmur/ambiyans ayarı |
| reducedMotion | bool = false | Kısmi hareket azaltma; bütün efektleri durdurmaz |
| visited | bool = false | Bahçeye girildiğinde true; ayrı ilk kullanıcı akışı/ödül açmaz |

GardenActivityState alanları: id (Guid N biçimli metin), kind (0 piknik, 1 bulut, 2 tekne), variant (0–2), step, boatCell (0–14), turns (9 adet 0–3), claimed. Bütün türler aynı kabı kullanır; örneğin piknikte turns ve boatCell oyun kuralını etkilemez. Valid, alan aralıklarını denetler; bütün alanlar arası mantıksal tutarlılığı kanıtlamaz.

**Oturumluk kalanlar:** Playing, seçili Tool, WindUntil, hayvanların taşınmış konumları, teknenin animasyon fazı, tutulan parmak/nesne, menü/sayfa, hata/ipuçları, fruitSequence, Sumi yürüyüşü ve efektler. Katalog sayfası veya açık modal uygulama yeniden açıldığında geri yüklenmez.

### 1.5 Dosyalar, göç ve yazma garantisinin sınırı

Normal kök G.Awake tarafından **Application.persistentDataPath** olarak seçilir:

- Bu Windows kurulumu: %USERPROFILE%/AppData/LocalLow/Rain Garden Development/Yagmur Bahcesi/.
- Bu Android testinde gözlenen yol: /sdcard/Android/data/com.raingarden.yagmurbahcesi.dev/files/.
- Asıl kayıt garden-v1.json; önceki geçerli kayıt .bak; ilk eski kayıt kopyası .v1-preserved; yazma ara dosyası .tmp.
- Albüm aynı kökte garden-album/*.png. Sunucuya yüklenmez.

Bu yollar ürün/company/paket kimliği değişirse aynı kalmak zorunda değildir. Yeni kimlikle kurulum, eski geliştirme uygulamasının kaydını kendiliğinden taşımaz.

Load sırası ana dosya → .bak → yeni başlangıçtır. .tmp ve .v1-preserved otomatik kurtarma kaynağı değildir. Geçerli v1 önce kopyalanır; üç büyüme korunur, eski renk 1 papatyaya, diğer değer kozmos türüne çevrilir, renk tonu 0 olur; diziler yeni uzunluklara tamamlanır. version=2 bellekte atanır; ilk sonraki başarılı yazmada diske geçer. Göç, geçmiş oyun davranışlarından bütün yeni keşifleri türetmez.

version>2 görülürse özgün dosya korunur, GardenSave ReadOnly olur ve ekrana başlangıç durumu döner. Bu mevcut bahçenin gerçekten silindiği anlamına gelmez. Uyarının oyuncuya görünürlüğünde açık sorun vardır; salt okunur oturumda ekonomik işlemler yazılamaz.

Write önce Validate yapar. Eski dosya geçerliyse .bak güncellenir; JSON .tmp'ye yazılır. Ana dosya varsa File.Replace, yoksa File.Move kullanılır. Replace desteklenmez veya IOException oluşursa File.Copy ile üzerine yazma yoluna geçilir. **Bu alternatif yol atomik değişim garantisi vermez; fsync, çok süreç kilidi ve journal yoktur.** .bak çoğu bozuk yazmadan dönüş sağlar; ani güç kesintisinde son işlemin hiç kaybolmayacağı test edilmiş değildir.

JSON düz metindir; şifreleme, imza/HMAC, sunucu doğrulaması veya hile önleme yoktur. Yerel oyunda pul gerçek paraya çevrilmez. Android işletim sisteminin yedekleme/cihaz aktarımı ayrı konudur: incelenen paket XML'inde allowBackup/fullBackupContent/dataExtractionRules açık ayarı görülmedi. Bu nedenle “işletim sistemi asla yedekleyemez” veya “kaldırıp kurunca kesin silinir” garantisi verilmez.

## 2. Genel oynanış ve ekranlar

### 2.1 İlk giriş

1. Unity tek sahneyi yükler; G.Awake kaydı yükler/doğrular, ek nesneleri kurar, ses/UI oluşturur, ApplyState çağırır.
2. **Her açılışta ana ekran** görünür. Kayıt olsa bile doğrudan yarım mini oyuna atlanmaz.
3. Ana ekranda oyun adı, Mina, Bahçeye gir, dil, ses ve Yetişkin düğmeleri bulunur. Hesap, izin formu, bağlantı veya ödeme adımı yoktur.
4. Bahçeye gir: Playing=true, visited=true, bahçe UI'sı ve ilk bulut ipucu; kayıt yazma denenir.
5. Çocuk serbest bahçe, üç dünya etkinlik düğmesi, Bahçem, Keşifler, Anılar, yardım ve araçları kullanabilir. Zorunlu sıra yoktur.

### 2.2 Ekran envanteri

| Ekran | Giriş / çıkış | Seçimler |
|---|---|---|
| Ana ekran | U.ShowHome; Bahçeye gir ile dünya | TR/EN, ses, yetişkin kapısı |
| Serbest bahçe | G.StartGarden; Menü veya Escape ile ana ekran | Dört araç ve bütün etkin dünya nesneleri |
| Yardım | ?; Hadi deneyelim veya × | Üç resimli temel ipucu; yeni koleksiyon öğreticisi yok |
| Etkinlik seçici | Yetersiz pul veya keşif yönlendirmesi | Üç mini oyundan seçim; dünyadaki düğmeler doğrudan da açar |
| Mini oyun | Dünya/seçici/keşif | Kendi girişi, yardım, Bahçeye dön, × |
| Sonuç | Mini oyun tamamlanınca aynı modal | Koleksiyona geç veya bahçeye dön; elle ödül talep düğmesi yok |
| Bahçem | Alt soldaki düğme veya sonuç | Çiçek/dekor sekmesi, sayfa, satın al/dik/yerleştir/kaldır |
| Dikim | Sahip olunan veya yeni alınan çiçek | Altı numaralı alan; dolu alana da ekim yapılabilir |
| Keşifler | Alt soldaki düğme | Dört projenin üçer işareti ve yönlendirme düğmesi |
| Anılar | Alt sağdaki düğme | Son altı resim, sayfa, yeni anı |
| Yetişkin kapısı | Ana ekran/bahçedeki Yetişkin | Üç matematik yanıtından biri |
| Yetişkin ayarları | Doğru yanıt | Dil, ses, hareket, bilgiler, sıfırlama |
| Gizlilik/bilgiler | Ayarlar | Metin ve ×; harici bağlantı açılmaz |
| Sıfırlama onayı | Ayarlar | Vazgeç veya Yeni bahçe |

Modal × ve Android geri/Escape, modalı kapatır; genel bir geri ekran yığını yoktur. Örneğin dikim ekranında × önceki katalog ekranına dönmez, dünyaya döner. Modal açıkken dünya girişi bloke edilir; dünya animasyonları ve ambiyans genel olarak sürer. Ana ekranda Escape kodda uygulamayı kapatmaz.

### 2.3 Kaç alternatif var?

**1 serbest bahçe + 3 mini oyun**; her mini oyun için **3 düzen**, toplam **9 düzen**. 4 proje × 3 bayrak, 12 keşiftir; bunlar 12 ayrı bölüm değildir. 8 çiçek × 3 tint, 24 görsel birleşimdir; 24 oyun mekaniği değildir. Seviye/XP, sıralama, oyun kazanma ekranı veya nihai final uygulanmadı.

Mini oyun düzeni oyuncu tarafından listeden seçilmez: activityPlays[kind] % 3. Bir türün tekrarları 0 → 1 → 2 → 0 döner. Düzenler hazırdır; prosedürel/rastgele bulmaca üreticisi yoktur. Piknikte üç farklı meyve sırası aynı kuralı kullanır.

## 3. Oyuncu aksiyonları ve sonuçları

Tablolarda **Y**: bellekte değiştirilip Save denenir; yazma başarısızsa otomatik geri alma yok. **T**: kopya üzerinde değiştir, dosyaya yaz, başarılıysa canlı State'e geçir. **O**: oturumluk. Her satır ilgili ekranda oyuncu tarafından ulaşılabilen bir davranışı tanımlar; doğrudan public metodun bütün geçersiz kullanım biçimleri UI aksiyonu sayılmaz.

### 3.1 Navigasyon, kontrol ve ayarlar

| ID | Aksiyon / ekran / koşul | Sistem değişikliği | Geri bildirim ve ilerleme |
|---|---|---|---|
| N01 | Bahçeye gir; ana ekran | Playing ve visited true, UI değişir; Y | Bulut ipucu/nota; pul verilmez |
| N02 | Menü; bahçe veya modal yokken Escape | Tutulan nesne iptal, Playing false, Y | Ana ekran; koleksiyon korunur |
| N03 | Araç seç; bahçe | Tool=Explore/Rain/Sun/Wind; O | Seçim rengi ve metin; seçim tek başına sulamaz/rüzgâr estirmez |
| N04 | ? yardım; bahçe | Modal açılır; O | Üç resimli anlatım; ilerleme yok |
| N05 | × / Bahçeye dön / Hadi deneyelim | Modal kapanır; bazı düğmeler ana UI'yı kurar; O | Dünya girişi geri açılır; yarım etkinlik korunur |
| N06 | Dil değiştir; ana ekran veya yetişkin | tr↔en; Y, UI yeniden kurulur | Metinler değişir; skor yok |
| N07 | Ses değiştir; ana ekran/bahçe/yetişkin | sound terslenir; AudioSource mute/ambiyans durdurma-başlatma; Y | Düğme metni değişir; oyun kuralları etkilenmez |
| N08 | Yetişkin düğmesi | 23–48 ile 17–37 arasındaki iki sayı toplanır; doğru şık rastgele | 3 cevap; hesap/kimlik/ebeveyn rızası doğrulaması değildir |
| N09 | Kapıda yanlış cevap | Yeni rastgele soru; O | Ceza/bakiye kaybı/deneme sınırı yok |
| N10 | Kapıda doğru cevap | Ayarlar açılır; O | Dil/ses/hareket/bilgi/sıfırlama erişimi |
| N11 | Hareket ayarı; yetişkin | reducedMotion terslenir; Y | Bazı salınım/kelebek/tekne hareketleri azalır; tam hareket kapatma değil |
| N12 | Gizlilik ve bilgiler | Bilgi modalı; O | Yerel saklama ve geçici yayıncı metni; hiçbir ağ isteği yok |
| N13 | Bahçeyi yeniden başlat | Onay modalı; O | Henüz veri değişmez |
| N14 | Vazgeç | Ayarlara dönüş | Durum korunur |
| N15 | Yeni bahçe; onay | Yeni GardenState yazılırsa State değiştirilir; ayarlar ve visited=true korunur | Büyüme, pul, açılan türler, keşif ve etkinlikler sıfırlanır. Albüm, eski yedek, hayvan konumları, Tool/WindUntil ayrıca sıfırlanmıyor |
| N16 | Pencere/uygulama odağını kaybet, arka plana al | Pointer iptal ve Y | Son yazılmış ilerleme yeniden açılabilir; mobilde OnApplicationQuit'e güvenilmez |
| N17 | İşletim sistemi üzerinden kapat | Normal çıkışta Save denenir | Genel oyun finali değildir; Windows native kapanış hatası açık |

Kaynaklar: G.StartGarden/Home/SelectTool/Toggle*/ResetGarden/OnApplication*; U.BuildHome/BuildGame/ShowGate/ShowParents/ConfirmReset.

### 3.2 Dünya nesneleri

Önkoşul: Playing=true, modal kapalı, nesne etkin ve seçilebilir. İşaretçi UI üstündeyse dünya aksiyonu başlamaz.

| ID | Aksiyon | Değişiklik / koşul | Geri bildirim ve ilerleme |
|---|---|---|---|
| W01 | Boş/filiz yatağa Keşfet veya Rüzgâr seçiliyken dokun | Water: min(2,growth+1); Y | Yağmur, Mina tepkisi, nota/ipucu; tam büyümeyle keşif 0 ve tür geçmişi |
| W02 | Yatağa Yağmur aracıyla dokun | Aynı Water; 2 zaten 2 kalır; Y | Büyümese bile efekt/kayıt; pul yok |
| W03 | Yatağa Güneş aracıyla dokun | Yalnız 1→2; 0/2 değişmez; Y | Parıltı; başarısız büyümede “önce yağmur” metni, açmış çiçekte de bu metin çıkar |
| W04 | Açmış çiçeğe Keşfet/Rüzgâr ile dokun | colors=(colors+1)%3; Y | Tint ve türe bağlı tepki; tür/sahiplik değişmez |
| W05 | Buluta dokun; herhangi araç | İlk plants<2 indeksi, hepsi doluysa rastgele yatak sulanır; Y | Yağmur; rastgele seçimin ekonomik etkisi yok |
| W06 | Bulutu sürükleyip bırak | En yakın yatağa uzaklık <4 ise Water; aksi yalnız RainAt/Ripple | Bulut kalıcı taşınmaz; uzaktaki yağmur bitki veya ödül üretmez |
| W07 | Tekneye dokun | boatLaunched=true, keşif 3; Y. Tool Wind ise ardından Wind | Gölete geçiş, dalga, Luli tepkisi |
| W08 | Tekneyi bırak | Gölet merkezine uzaklık <3 ise LaunchBoat; aksi eski konum | Hatalı bırakmada ceza yok; mini oyun başlamaz |
| W09 | Gölete dokun | Wind aracı: Wind; diğerlerinde yalnız Ripple/Luli/nota | Wind tekne yoksa da önce suya yollar |
| W10 | Rüzgâr çalıştır | WindUntil=Time.time+5, keşif 5; Y | 5 saniye hızlı salınım, yaprak izi; süre yalnız oturumluk |
| W11 | Elma/armuda dokun | ShareFruit(1/2), ilk boş picnic alanı; doluysa indeks 0; Y | Pufi/Kori tepkisi, örtüde meyve, keşif 6; mini oyun/pul etkilenmez |
| W12 | Meyveyi örtüye/dosta bırak | Örtü merkezine <2,6 veya **ilk dört dosta** <1,5 birim ise ShareFruit | Kaynak meyve geri gelir; Sumi bırakma hedefi listesine dahil değil |
| W13 | Meyveyi geçersiz yere bırak | İlerleme değişmez | Kaynak yerine döner, örtü/dost ipucu |
| W14 | Piknik örtüsüne dokun | Oturumdaki fruitSequence ile 1,2,1,2...; Y | Normal paylaşma; açlık/kişiye özel envanter yok |
| W15 | Dosta dokun | React ve nota; O | Luli+suya inmiş tekne: dalga; Sumi: yürüyüş; diğerleri parıltı |
| W16 | Dostu sürükleyip bırak | x [-7,8;7,8], y [-3,1;1,8] içine rest; O | Tepki; yeniden uygulama açılışında başlangıç konumu |
| W17 | Dekoru sürükle | x [-7,8;7,8], y [-3,7;1,4]; Y | Konum kalıcıdır. Save.Validate y üst sınırı ayrıca 1,8; iki sınır aynı değil |
| W18 | Dekora dokun | Her türde kısa tepki/nota/parıltı; ek davranış aşağıda | Ek davranışa göre keşif/bitki/piknik değişebilir |
| W19 | İkinci parmakla dünya nesnesi tutmaya çalış | finger doluyken PointerDown reddeder | Aynı anda iki dünya nesnesi sürüklenmez |
| W20 | Sürüklemeyi iptal/odağı kaybet | holdStart ve çizim sırası geri yüklenir | Başarılı Drop sayılmaz |
| W21 | Etkileşimsiz arka plana dokun | Hit yoksa kalıcı değişiklik yok | Dekor çizimleri otomatik etkileşim nesnesi olmaz |

**Araç matrisi:** Yağmur/Güneş yalnız yatağa dokunma dalını değiştirir; diğer nesneler kendi davranışını sürdürür. Rüzgâr aracında çiçeğe dokunmak da keşif dalını kullanır. Araçlar bağımsız sahneler/modlar değildir.

### 3.3 Koleksiyon, projeler, mini oyunlar ve albüm

| ID | Aksiyon / önkoşul | Durum değişikliği | Geri bildirim / sonuç |
|---|---|---|---|
| C01 | Bahçem aç | Modal, varsayılan çiçek ilk sayfa | Pul bakiyesi, dört kart |
| C02 | Çiçek/Süs sekmesi veya sayfa oku | UI sayfası; O | 8 çiçek 2 sayfa, 12 dekor 3 sayfa; oklar döngüsel |
| C03 | Kilitli türe dokun; pul yetersiz | Satın alma yok | Etkinlik seçici açılır; hangi türe dönüleceği otomatik hatırlanmaz |
| C04 | Kilitli türe dokun; yeterli pul | Maliyet düşümü+sahiplik T; yeni çiçekte keşif 1 | Çiçekte dikim ekranı; dekorda ayrıca yerleştirme işlemi |
| C05 | Sahip olunan çiçeği Dik | Henüz veri değişmez | Altı alan seçimi; ücretsiz |
| C06 | Dikim alanı seç | Tür atanır, growth=1, tint=0; T. Yeni türse keşif 2 | Eski büyüme/tint yerini yeni filize bırakır; eski tür sahipliği kaybolmaz |
| C07 | Dikimden × ile çık | Önceden satın alınmış tür sahipliği ve harcanan pul korunur | Dikim gerçekleşmez; sonra ücretsiz dikilebilir |
| C08 | Sahip olunan dekoru Kaldır/Yerleştir | placed terslenir; T; yeni dekoru koyunca keşif 8 | Görünürlük değişir; konum ve sahiplik korunur; ücret/iade yok |
| C09 | Keşifler aç | Dört proje kartı; O | 12 işaret, ilerleme; ek ödül verilmez |
| C10 | Proje yönlendirme düğmesi | İlk eksik adıma göre katalog/etkinlik/ipucu | Tamamlanmış kartlarda sonraki projeye taşan yönlendirme kusuru var |
| M01 | Etkinlik düğmesi/seçici | Begin: yarımını kullan veya yeni GUID/düzen üret; T | Uygun mini oyun modalı; kayıt başarısızsa başlamaz |
| M02 | Piknikte meyve seç | Sıradakiyle aynıysa step++; T | Doğru kart ilerler; yanlışsa durum/ödül değişmez, başka yol ipucu |
| M03 | Bulutta kanala dokun | turns[cell]=(turns+1)%4; T | Kanal yönü ve bağlantı rengi değişir; taş hücre devre dışı |
| M04 | Teknede ok seç | Sınır/taş geçerliyse hücre değişir; güncel hedefse step++; T | Tekne yeni hücrede, iskele ilerlemesi; geçersizde ceza yok |
| M05 | Birlikte yapalım | Kurala göre bir yardım adımı; T | Normal tamamlama ile aynı 1 pul; sınırsız yardım |
| M06 | Etkinlikten çık veya uygulamayı yeniden aç | Son yazılmış run korunur | O türü tekrar seçince kaldığı yer; uygulama başlangıcı yine ana ekran |
| M07 | Son adımı tamamla | Adım kaydı ve Claim ayrı T işlemleridir | Sonuç modalı; Claim başarılıysa 1 pul, sayaçlar ve keşif |
| M08 | Sonuçtan Bahçem/bahçe | Modal değişir/kapanır | Ödül ayrıca düğmeye basmayı beklemez |
| M09 | Aynı türü tekrar aç; önceki claimed=true | Yeni GUID; bir sonraki variant; T | Tekrar oynama; önceki sonucun kopyası ödül vermez |
| H01 | Anılar aç | PNG listesi alınır, en yeni adlı dosya yüklenir | Boşsa açıklama; okumada bazı hata yolları eksik |
| H02 | Albüm okları | Sayfa döngüsü; O | Oranı korunarak önceki/sonraki görüntü |
| H03 | Yeni anı kaydet | Modal kapanır; frame sonunda PNG; altıdan eski dosyalar silinir | Görünür bahçe ve HUD da resme girer; pul/keşif yok, kameraya erişilmez |

Kaynaklar: X.ShowCollection/ShowPlanting/ShowProjects/GuideProject/OpenActivity/ActivityAction/ShowActivityReward/ShowAlbum; P.Purchase/Plant/ToggleDecoration/ActivityInput/Snapshot.

### 3.4 Bütün türlerin fiyat ve davranışları

Çiçek renk çarpanları: 0=(1,1,1), 1=(0,83;0,92;1), 2=(1;0,86;0,82). Tür başına ayrı üç görsel dosya yoktur.

| Kimlik / ad | Açılma maliyeti | Açmış çiçeğin dokunma tepkisi |
|---|---:|---|
| cosmos / Kozmos | Başlangıçta açık | Kelebek ve Mina tepkisi |
| daisy / Papatya | Başlangıçta açık | Kısa ölçek tepkisi ve parıltı |
| bluebell / Çan çiçeği | 1 | Nota; nota seçimi yatak indeksinden gelir |
| lavender / Lavanta | 2 | Kelebek ve Mina tepkisi |
| tulip / Lale | 2 | Kısa ölçek tepkisi ve parıltı |
| sunflower / Ayçiçeği | 2 | Nota |
| lily / Nilüfer | 2 | Dalga ve Luli tepkisi; suya dikme zorunluluğu yok |
| poppy / Gelincik | 2 | Kelebek ve Mina tepkisi |

Kelebekler bu gruptaki çiçek tam büyüyünce P.LateUpdate ile görünür; sadece ilk dokunuştan sonra açılan ayrı koleksiyon öğeleri değildir.

| İndeks / kimlik / ad | Maliyet | Genel tepkiye ek işlem |
|---|---:|---|
| 0 pinwheel / Rüzgârgülü | Açık | Wind |
| 1 stones / Denge taşları | Açık | Yok |
| 2 mushroom / Mantar | Açık | Yok |
| 3 chime / Rüzgâr çanı | 1 | Wind |
| 4 birdhouse / Kuş evi | 2 | Luli React(3); yeni kuş üretilmez |
| 5 bench / Bahçe bankı | 2 | Yok; oturma sistemi yok |
| 6 lantern / Fener | 2 | Beş parıltı dalgası, araları 0,3 saniye |
| 7 can / Sulama kabı | 1 | NextPlot + Water |
| 8 bridge / Minik köprü | 3 | Yok; geçiş fiziği yok |
| 9 pot / Saksı | 1 | Yok; yedinci dikim yuvası açmaz |
| 10 fountain / Çeşme | 3 | Dalga |
| 11 basket / Piknik sepeti | 2 | Elma paylaşma |

Yeni çiçeklerin toplamı **11**, dekorların toplamı **17**, bütün kilitlerin toplamı **28 pul**. Fiyat sabittir, indirim/rasgele ödül/gerçek parayla pul yoktur. Teorik olarak 28 tamamlama yeterlidir; bu içerik süresi veya bağlılık garantisi değildir. Katalog tamamlanınca özel final veya pul gizleme davranışı yoktur.

## 4. Algoritmik oyun akışı

### 4.1 Büyüme, seçim ve sürükleme

- Rain: indeks geçerliyse growth=min(2,growth+1).
- Sun: yalnız growth==1 olduğunda 2; diğerlerinde false.
- Plant: sahip olunan tür + 0–5 alan → growth=1. Başlangıç tohum durumunu atlar.
- RememberFlower: growth==2 ise keşif 0; tür ilk defaysa plantedSpecies'e ekle; farklı tür sayısı ≥3 ise keşif 10. Aynı anda üç tür görünmesi gerekmez.
- Hit: OverlapPointAll içinden en yüksek çizim sırasını seç. Bulut 50, tekne/meyve 40, dost 30, yatak/dekor 20, örtü -40, gölet -50. Eşit sıralı çakışmalarda ek bağlayıcı öncelik yoktur.
- PointerDown yalnız boş finger alanını alır; başlangıç, görsel offset ve nesne rest'i kaydedilir.
- Hareket eşiği max(12 piksel, ekran yüksekliği ×0,015). Aşılırsa draggable nesne sürüklenir, sıra 90 olur.
- PointerUp önce rest/çizim sırasını geri koyar; iptal değilse moved durumuna göre Drop veya Interact çağırır; pointer alanları temizlenir.
- Yataklar sürüklenebilir değildir: bu eşiğin davranışı draggable nesneler içindir.

### 4.2 Ortak mini oyun ve ekonomi akışı

~~~mermaid
flowchart TD
    Start["Uygulama açılır"] --> Load["Load + Validate + dünya/UI kurulumu"]
    Load --> Home["Ana ekran"]
    Home --> Play["Bahçeye gir"]
    Play --> Choice{"Oyuncunun seçimi"}
    Choice --> Free["Serbest bahçe: büyüt / taşı / paylaş"]
    Free --> SaveY["Bellek değişir, Save denenir"]
    SaveY --> Choice
    Choice --> Begin["Mini oyun Begin: yarımı sürdür veya yeni run"]
    Begin --> Write{"Kopya kayıt yazıldı mı?"}
    Write -- Hayır --> Error["Başlatma uygulanmaz; hata bildirimi denenir"]
    Error --> Choice
    Write -- Evet --> Resume{"Run zaten tamam mı?"}
    Resume -- Hayır --> Input["Meyve / kanal / yön / yardım"]
    Resume -- Evet --> Claim["Claim: id aynı, tamam, claimed değil"]
    Input --> Valid{"Hamle geçerli mi?"}
    Valid -- Hayır --> Input
    Valid -- Evet --> Persist["Adımı T ile kaydet"]
    Persist --> Persisted{"Yazma başarılı mı?"}
    Persisted -- Hayır --> StepError["Eski state korunur; hata bildirimi"]
    StepError --> Input
    Persisted -- Evet --> Done{"Complete?"}
    Done -- Hayır --> Input
    Done -- Evet --> Claim
    Claim --> RewardSave{"Ödül yazıldı mı?"}
    RewardSave -- Hayır --> Pending["Tamamlanmış run bekler; yeniden açınca dene"]
    RewardSave -- Evet --> Reward["claimed=true, 1 pul, sayaçlar, keşif"]
    Pending --> Choice
    Reward --> Result{"Sonuç ekranındaki seçim"}
    Result -- Bahçe --> Choice
    Result -- Bahçem --> Catalog["Koleksiyon"]
    Choice --> Catalog
    Catalog --> Owned{"Öğe açık mı?"}
    Owned -- Evet --> Place["Dikim veya dekor görünürlüğü ayrı T"]
    Owned -- Hayır --> Funds{"Yeterli pul var mı?"}
    Funds -- Hayır --> ChooseActivity["Etkinlik seçimi"]
    ChooseActivity --> Begin
    Funds -- Evet --> Purchase["Ücret ve sahiplik birlikte T"]
    Purchase --> Bought{"Yazma başarılı mı?"}
    Bought -- Hayır --> Catalog
    Bought -- Evet --> Place
    Place --> Choice
    Input -- Çık --> Choice
    Catalog -- Kapat --> Choice
    Choice -- Menü --> Home
    Choice -- Arka plan --> Pause["Pointer iptal; Save denenir"]
    Pause -- Devam --> Choice
    Choice -- Uygulamayı kapat --> Exit["Süreç sona erer; normal çıkışta Save denenir"]
    Exit -- Yeniden aç --> Start
~~~

Şema işlem bağlantılarını gösterir; kayıt hatasının mesajının her ekranda gerçekten görünür olduğu anlamına gelmez. Ayrıca pause/quit işlemi ana ekran veya modal açıkken de gelebilir; açık modalın oturum içinde korunması yeniden uygulama açılışındaki ana ekran davranışından ayrıdır. Dosya kurtarma ve read-only başlangıç dalları bölüm 1.5'te tanımlıdır.

T işlemi: Clone (JSON roundtrip+Validate) → mutation → Validate → Write → State=next → ApplyState. Mutation false ise yazma/uygulama yapılmaz. Yazma false ise mevcut State korunur. G.Save kullanan Y yollarında bu geri alma yoktur.

Claim yalnız **aktif etkinlikte** aynı runId, complete=true, claimed=false iken çalışır. Aynı kayda claimed, stamps+1, activityCompletions+1, activityPlays[kind]+1 ve ilgili keşif yazılır. Sayaç güncellemesi ödül kaydına bağlı olduğundan aynı run sonraki yüklemede tekrar kazanılmaz. Sonsuz geçmiş işlem defteri tutulmaz.

Tamamlayan adım ile ödül iki ayrı yazmadır. Aralarında kapanırsa completed/unclaimed run kalabilir; X.ShowActivityReward açılırken Claim tekrar denenir. Başarılı Claim'den sonra kapanırsa ödül tekrar verilmez. Sonuçta “yeniden dene” metni olsa da ayrı retry düğmesi yoktur; çıkıp o etkinliği yeniden açmak gerekir.

### 4.3 Piknik Sepeti

kind=0; meyve 1 elma, 2 armut:

| variant | Dizi | Hedef adım |
|---:|---|---:|
| 0 | 1,2,1 | 3 |
| 1 | 2,2,1,1 | 4 |
| 2 | 1,2,1,2,1 | 5 |

PickFruit: henüz bitmemişse ve seçilen meyve order[step] ise step++. Yanlış seçim step'i azaltmaz; süre/puan cezası yok. Help doğrudan order[step]'i seçer. Complete: step>=Goal ve Valid. Oyun, serbest bahçedeki picnic[4] dizisini doldurmaz; yalnız ortak ödül/keşif sonucuyla bağlanır.

### 4.4 Bulutun Yolu

kind=1. Hücreler üstten alta, soldan sağa 0–8. Yön bitleri N=1, E=2, S=4, W=8.

| variant | Yol hücreleri | Her yol hücresinin hedef maskesi |
|---:|---|---|
| 0 | 0,1,4,7,8 | 10,12,5,3,10 |
| 1 | 3,4,1,2,5 | 10,9,6,12,3 |
| 2 | 6,3,4,5,2 | 9,6,10,9,6 |

Başlangıç dönüşleri bütün hücrelerde 1+i%3: [1,2,3,1,2,3,1,2,3]. Etkin kanal dokunuşu dönüşü mod 4 artırır. Bir dönüşün maskesi ((mask << 1) & 15) | (mask >> 3).

Connected, güncel maskenin **o hücrenin hazır hedef maskesine eşitliğidir**. Komşular üzerinden çalışan genel bir ağ çözümü veya sıvı simülasyonu değildir. Düz kanal iki eşdeğer yönelimde doğru sayılabilir. Complete beş yol hücresinin hepsi Connected ise true; step bu oyunda ilerlemez. UI'da su rengi yalnız o hücreye kadarki hedefler doğruysa etkinleşir. Yol dışındaki taşlar döndürülemez.

Help yol sırasındaki ilk yanlış hücrenin turns değerini 0 yapar; bu tek 90° dönüş olmak zorunda değildir. Hedef maskeler 0 dönüşte tanımlı olduğundan hücre doğrudan düzelir.

### 4.5 Yaprak Kaptanı ve BFS yardımı

kind=2. Hücre i için x=i%5, y=i/5; üst sıra 0–4, orta 5–9, alt 10–14. Yön kodları 0 yukarı, 1 sağ, 2 aşağı, 3 sol.

| variant | Başlangıç | Sırayla ziyaret edilecek iskeleler | Taşlar |
|---:|---:|---|---|
| 0 | 10 | 12,4 | 6,8 |
| 1 | 0 | 2,14 | 6,7 |
| 2 | 10 | 0,9,14 | 5,12 |

NextCell sınır/taş kontrolü yapar; uygunsuz yön/hücre -1 döner. Sail geçerli hücreye gider; **yalnız stops[step]** hedefindeyse step artar. Sonraki iskeleye erken gitmek veya eskisini tekrar ziyaret etmek puan vermez. Complete hedef sayısına ulaşmaktır.

Help, mevcut hücreden güncel hedefe BFS (genişlik öncelikli arama) yapar; kuyruğa komşuları yukarı/sağ/aşağı/sol sırasıyla alır. parent ve direction dizileriyle yolu geriye izleyip **ilk bir adımı** uygular. Eşit uzunluklu yolların tercihi bu yön sırasından gelir. Normal hazır haritalarda çözülebilirlik test edilmiş; bozulmuş kaydın bütün mantıksal durumları test edilmemiştir. Grafik en fazla 15 düğüm/4 komşu; arama O(V+E), bellek O(V).

### 4.6 Projeler ve açılma koşulları

| Bayrak | Proje | Gerçek tetikleyici |
|---:|---|---|
| 0 | Mina | Water/Sun sonunda herhangi çiçek growth=2 |
| 1 | Mina | flowerIndex≥2 olan yeni tür satın alınır |
| 2 | Mina | flowerIndex≥2 tür bir alana dikilir; büyümesi şart değil |
| 3 | Luli | Serbest bahçe LaunchBoat |
| 4 | Luli | Yaprak Kaptanı ödülü Claim edilir |
| 5 | Luli | Serbest bahçe Wind |
| 6 | Pufi | Serbest bahçede ShareFruit |
| 7 | Pufi | Piknik Sepeti ödülü Claim edilir |
| 8 | Pufi | Yeni dekor (indeks≥3) görünür olacak şekilde yerleştirilir |
| 9 | Sumi | Bulutun Yolu ödülü Claim edilir |
| 10 | Sumi | RememberFlower geçmişinde ≥3 farklı tam büyümüş tür |
| 11 | Sumi | Üç activityPlays değerinin her biri >0 |

Progress ilgili üç true değerinin sayısıdır. Sıra zorunluluğu yoktur; ilk üç projede count kadar sıradaki görsel parça görünür. Görsel parça ile tamamlanan belirli adım bire bir bağlı değildir. Sumi yalnız 9,10,11 tamamlanınca görünür; bütün dört projeyi bitirme koşulu yoktur. Proje tamamlama ek pul/XP vermez.

**Uçtan uca örnek:** bir kozmos büyüt → Piknik Sepeti'nden 1 pul → çan çiçeğini aç/dik/büyüt → ücretsiz papatyayı başka alanda büyüt → Bulut ve Tekne mini oyunlarını tamamla → Sumi açılır. Serbest teknede rüzgâr, meyve paylaşma ve 1 pulluk çanı yerleştirme ile kalan projeler de tamamlanabilir. Üç mini oyun ödülü ve bu iki alışveriş sonunda bakiye 1'dir; Windows smoke bu örneği sınar.

### 4.7 Sonlandırma, tekrar ve algoritmaların kapsamı

Oyun genelinde “kazandın/kaybettin” bitiş durumu yoktur. Mini oyun tamamlanır; bahçe sürer. Bütün katalog/projeler bitse bile serbest oyun ve pul kazanımı devam eder. Ölüm, can, enerji, geri sayım, günlük/haftalık takvim, streak ve bildirim yoktur.

Rastgelelik: ebeveyn sorusu/şık yeri, bütün yataklar doluyken bulut hedefi, efekt konumları. Ekonomi ve mini oyun düzenleri rastgele değildir. Esas veri boyutları küçük sabittir; katalog aramaları lineer, Hit çakışan collider sayısına bağlıdır. Kayıt/klonlama JSON boyutu ve dosya I/O'suna bağlıdır; ana iş parçacığında gerçekleşir.

## 5. Oyun senaryoları ve test edilebilir durumlar

Sonsuz dokunma dizileri, sürekli konumlar ve işletim sistemi olaylarının bütün birleşimleri tek tek listelenemez. Test eki, mevcut kodun kullanıcıya açık dallarını; her mini oyunun bütün hazır düzenlerini; kayıt, kesinti, yanlış giriş ve sınır değer eşdeğerlik sınıflarını kapsar. Bu, matematiksel olarak bütün olası durumların test edildiği iddiası değildir.

Beklenen sonuç kaynakları: normal kullanıcı akışında bu belgenin bölüm 3–4'ü; bozuk/eksik davranışlarda test ekindeki **istenen kabul sonucu**. Bir test bugün kodun bilinen kusurunu üretirse “koda uygun” diye başarılı sayılmaz. Bütün modlar için başlangıç/yarım/tamamlandı-ödülsüz/ödüllü/yeniden başladı durumları ayrı sınanmalıdır.

## 6. Test ve kalite kontrol planı

[Uygulanabilir test matrisi](TEST_SENARYOLARI_VE_KABUL_KRITERLERI.md) her testin önkoşulunu, adımlarını, beklenen sonucu, gözlenecek veriyi ve geçme şartını verir. [CSV çalışma formu](QA_TEST_KAYIT_SABLONU.csv) yeni test oturumunun gerçek sonuçlarını doldurmak içindir.

Mevcut kanıt: 150 Editor assertion; Windows'ta iki oran için 29'ar işlev assertion'ı; Android API36 X86_64/4 KB emülatörde 22 kontrol; ARM64 APK'nın izin/hizalama denetimi. **Windows süreci iki testte de başarısız kapanmıştır.** ARM64 fiziksel cihaz veya 16 KB çalışma testi, aile testi ve bütün yeni sınır senaryoları geçmiş sayılmaz. Bu doküman hazırlanırken bu oyun testleri yeniden çalıştırılmadı; var olan çıktılar incelendi ve APK hash'i doğrulandı.

## 7. Eksikler, riskler ve iyileştirmeler

[Kod denetimi kaydı](KOD_DENETIMI_VE_ACIK_KONULAR.md), dört istenen durum sınıfını ve kaynaklı açık bulguları ayırır. Öncelikli alanlar: Windows kapanışı; kayıt hatasının görünürlüğü/geri alma tutarlılığı; yeni sürüm kayıt korumasının UX'i; modal sırasında tutulan parmak; tamamlanmış proje yönlendirmesi; reset kapsamı; cihaz/erişilebilirlik/performans ve release ayarları.

Bu belge hata düzeltme olarak kabul edilmemeli; mevcut davranış ve düzeltme kabul kriteri birbirinden ayrıdır.

## 8. Release/deployment hazırlığı

[Release çalıştırma planı](RELEASE_CALISTIRMA_PLANI.md) bağımlılık sırasını, mevcut komutları ve henüz bulunmayan production adımlarını tanımlar. Bugün BuildAndroid yalnız test APK üretir; otomatik AAB/upload/release hattı yoktur. B.Setup üretim için elle değiştirilen kimlik/sürüm ayarlarını geri yazabildiğinden release yolu ayrıca hazırlanmalıdır.

Mevcut çevrimdışı mimaride DB/sunucu kurulumu yoktur. Gizlilik sayfası ve destek kanalı oyun backend'i değildir; yayıncı tarafından hazırlanacak dış bağımlılıklardır. Roblox ve iOS bu Android release'in parçası değildir; Roblox yalnız plan belgesinde, iOS ertelenmiştir.
