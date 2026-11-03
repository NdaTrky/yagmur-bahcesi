# Oyunu açma ve arkadaşlarla test etme

3 Kasım 2026. Bu belge hızlı kullanım içindir; ayrıntılı teknik dokümanları okumadan oyunu açabilirsin.

## Bilgisayarında hemen oynamak

Proje ana klasöründeki [OYUNU_AC.cmd](../OYUNU_AC.cmd) dosyasına çift tıkla. Bu, Windows oyununu açar; Unity veya Hub'ı açman gerekmez.

Bu dosya **son derlenmiş oyunu** çalıştırır. C# dosyalarını değiştirmek mevcut EXE'yi otomatik güncellemez. Yeni değişiklikleri Editor'da Play ile deneyebilir, yeni Windows çıktısı için Editor aynı proje üzerinde açık değilken BuildWindows çalıştırabilirsin.

Bilinen sınırlama: bu Windows test sürümünün kapanışında önceki testlerde UnityPlayer.dll çökmesi gözlendi. Oynanış kontrolü ve temiz kapanış ayrı sonuçlardır; hata düzelmiş sayılmıyor.

## Unity Editor'da projeyi açmak

[UNITY_PROJESINI_AC.cmd](../UNITY_PROJESINI_AC.cmd) dosyasını ekledik. Çift tıklayınca Unity **6000.3.17f1** ile projeyi ve Garden sahnesini açma komutu çalışır.

Başlatıcı bu bilgisayarda çalıştırıldı: Editor başlığında **YagmurBahcesi - Garden** görüldü; Garden.unity yüklü ve aktif sahne olarak doğrulandı. Oyun otomatik Play moduna alınmadı; üstteki ▶ ile başlatabilirsin.

Unity Hub'a proje, **Add → Add project from disk** üzerinden **YagmurBahcesi** adıyla eklendi. İleride Hub listesi sıfırlanırsa aynı işlemde şu klasörü seç:

~~~text
C:\Users\nturk\OneDrive\Masaüstü\oyun\YagmurBahcesi
~~~

Seçilecek kök Assets, Packages ve ProjectSettings klasörlerini içerir. Üstteki oyun klasörünü veya Android derlemesi için oluşturulan UnityBuilds kopyasını seçme. [Unity Hub proje ekleme](https://docs.unity.com/en-us/hub/projects-window-reference)

1. Hub → Projects → YagmurBahcesi satırına tıkla; alternatif olarak yeni CMD'yi aç.
2. Açık sahne Garden değilse Project panelinden **Assets → RainGarden → Scenes → Garden** sahnesine çift tıkla.
3. Üstteki **▶ Play** düğmesine bas; Game sekmesinden oyna.
4. Durdurmak için Play'e yeniden bas. Play sırasında sahne nesnelerinde yapılan geçici düzenlemelerle, oyunun JSON'a yazdığı kalıcı ilerleme farklıdır.
5. Yeni kodu arkadaşlarınla paylaşmak için ilgili platformu yeniden derlemek gerekir. Editor'da görmek, gönderdiğin eski ZIP/APK'nin değiştiği anlamına gelmez.

Başlatıcı Unity'nin openfile argümanıyla Garden sahnesinin yolunu verir; proje dosyalarını yeniden üretmez. Aynı projeyi kullanan Editor açıkken ayrıca batch build başlatma. [Unity Editor komut satırı](https://docs.unity.com/en-us/engine/6000.0/manual/unity-editor/command-line-arguments/editor)

## Arkadaşına bugün gönderebileceğin dosyalar

| Arkadaşının cihazı | Gönderilecek | Yapacağı işlem |
|---|---|---|
| Windows PC | [Hazırlanan Windows ZIP](../Builds/Paylasim/20261103-230109-52ff04/YagmurBahcesi-Windows-test.zip) | ZIP'i tamamen çıkarır, içindeki OYUNU_AC.cmd'ye çift tıklar; Unity gerekmez |
| Android 8+ ARM64 telefon/tablet | [Android test APK](../Builds/Paylasim/20261103-230109-52ff04/YagmurBahcesi-Android-test.apk) | APK'yi indirip cihazının izin verdiği test kurulumu akışıyla yükler; Unity gerekmez |
| Mac/iPhone veya kurulum istemeyen kullanıcı | Henüz hazır dosya/URL yok | Aşağıdaki Web sürümü hazırlanmalı; Windows ZIP ve Android APK bu cihazların yerel uygulaması değildir |

Windows ZIP yaklaşık **46,8 MB**, APK **29,1 MB**. ZIP bütün Windows çalışma dosyalarını ve açılış notunu içerir. Yalnız EXE göndermek yeterli değildir. Paketler test sürümüdür; farklı arkadaş cihazlarında henüz doğrulanmamıştır.

[Paket manifesti](../Builds/Paylasim/20261103-230109-52ff04/paket-bilgisi.json), dosya boyutlarını ve SHA-256 değerlerini içerir. Bu paketteki 144 Windows kaynak dosyasının arşivden okunan içeriği SHA-256 ile asıllarıyla eşleştirildi; iki ek dosyayla toplam 146 ZIP girdisi var. APK kopyasının hash'i de doğrulandı. Bu işlem yeniden derleme veya bütün oyun testlerini tekrar çalıştırma değildir.

Dosyaları kendi seçtiğin dosya paylaşım hizmetine yükleyip indirme bağlantısı gönderebilirsin. Bu çalışmada dışarıya dosya yüklenmedi ve arkadaşlara mesaj gönderilmedi. APK'nin oyun kodunda reklam/hesap/ağ oyunu yok; her oyuncunun bahçesi kendi cihazındadır.

Yeni build aldıktan sonra yeni paylaşım paketi oluşturmak için proje kökünde:

~~~powershell
.\tools\Prepare-SharePackage.ps1
~~~

Araç **mevcut** Builds/Windows ve Android test APK'sını, Builds/Paylasim altında yeni zaman damgalı klasöre paketler. Build veya yayın yapmaz; eski paylaşım paketlerini değiştirmez. Hangi sürümü göndereceğini yeni klasördeki paket-bilgisi.json ile eşleştir.

## Expo Go neden kullanılamıyor?

Bu proje **Unity/C#** ile geliştirilmiş. Expo Go, React Native/JavaScript uygulamalarını kendi içerdiği native kütüphanelerle çalıştırır; mevcut Unity projesini veya APK'yi QR koduyla doğrudan içine yükleyemeyiz. Unity'yi özel bir native uygulamaya gömmek ayrı geliştirme olur ve yine Expo Go'nun hazır uygulamasını kullanmak anlamına gelmez. [Expo Go sınırları](https://docs.expo.dev/faq/)

Bu oyun için benzer kolaylıkta paylaşımın yolu Unity **Web** çıktısıdır: kullanıcı bağlantıyı açar, oyun tarayıcıda yüklenir.

## Port veya internet bağlantısıyla oynatma

**Mümkün; önce oyunun Web sürümü oluşturulmalı.** Mevcut Windows EXE/Android APK bir web sunucusu değildir. Bunlara yalnız port açmak tarayıcıda oynatmaz.

3 Kasım kontrolünde Unity kurulumunda Android ve Windows destekleri var; **Web Build Support kurulu değil**, Web build yöntemi ve tarayıcı test çıktısı yok. Aşağıdaki adımlar bu çalışmada yapılmış sayılmaz:

1. Unity 6000.3.17f1 için Web Build Support modülünü kur.
2. Android ayarlarını koruyan ayrı bir Web Build Profile/build yolu oluştur.
3. Yerel kayıt ve albümün tarayıcı depolamasını, ekran yakalamayı, ilk kullanıcı etkileşimi sonrası sesi, dokunmayı ve bellek kullanımını test et. Dosya tabanlı kodun tarayıcıda aynen kalıcı çalıştığı varsayılmaz; Web persistentDataPath tarayıcının IndexedDB alanını kullanır. [Unity Web kalıcı veri yolu](https://docs.unity.com/en-us/engine/6000.0/script-reference/unityengine/application/persistentdatapath)
4. Web çıktısını yerel sunucuda çalıştır; sıkıştırma/MIME başlıklarını build ayarıyla eşleştir.
5. Gerçek tarayıcı testi geçince yalnız Web çıktı klasörünü uygun bir hosta yükle veya geçici bir geliştirme tüneliyle paylaş. Şu anda başlatılmış sunucu, açılmış port veya paylaşılabilir oyun URL'si yoktur. [Unity Web geliştirme ve yayın akışı](https://docs.unity.com/en-us/engine/6000.3/manual/platform-specific/webgl/intro/gettingstarted)

| Bağlantı türü | Kim açabilir? | Koşul |
|---|---|---|
| localhost:8080 | Sunucunun çalıştığı bilgisayar | Web build + çalışan yerel sunucu |
| Yerel IP:8080 | Aynı ağdaki erişebilen cihazlar | Sunucu uygun arayüze bağlı ve ağ/firewall erişimi uygun |
| Geçici HTTPS tüneli | Linki verdiğin uzaktaki kişiler | Yerel sunucu ve tünel açık; bilgisayar çalışıyor |
| Barındırılmış HTTPS adresi | Linki verdiğin kişiler | Web build bir hostta; senin bilgisayarın açık olmak zorunda değil |

Port 8080 yalnız örnektir; hazır bir adres değildir. **localhost linkini arkadaşına göndermek onun bilgisayarını senin oyununa bağlamaz.** Hosting/tünel yalnız derlenmiş Web klasörünü sunmalı; kaynak proje ve özel dosyalar sunucu kökü yapılmaz.

Unity 6.3 belirli masaüstü ve mobil tarayıcıları destekler; bu oyunun iPhone/Android tarayıcı performansı ve kayıt davranışı henüz test edilmedi. Web önizlemesi Android APK'nin cihaz kabul testlerinin yerine geçmez. [Unity 6.3 tarayıcı desteği](https://docs.unity.com/en-us/engine/6000.3/manual/platform-specific/webgl/intro/browsercompatibility)

Bağlantıyla erişim çok oyunculu özellik eklemez: herkes kendi bahçesini oynar. Ortak bahçe/aynı oturum için ayrıca ağ ve sunucu geliştirmesi gerekir.

Bugün hızlı geri bildirim için hazır ZIP/APK kullanılabilir. Kurulum istemeyen ve farklı cihazlardan katılacak testçiler için uygun sonraki geliştirme Web önizlemesidir.
