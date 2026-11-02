# Yağmur Bahçesi — ilk oynanabilir sürüm

Unity 6000.3.17f1 ile geliştirilmiş, 4–6 yaş için tasarlanan sakin bir bahçe oyunu. İlk hedef Android; iOS ertelendi. Bu depo ticari yayını tamamlanmış ürün değildir.

## Hemen oyna

Bu bilgisayarda **`OYUNU_AC.cmd` dosyasını çift tıkla**. Windows önizlemesi açılır; Unity Editor gerekmez. `Builds/Windows` klasörünün tamamı birlikte tutulmalıdır.

- Bahçeye gir; buluta veya toprağa dokunarak çiçek büyüt.
- Güneş aracını seçip filize dokun; açılmış çiçeğe keşif modunda dokunarak rengini değiştir.
- Yaprak tekneyi gölete sürükle; rüzgâr aracını dene.
- Elma veya armudu piknik örtüsüne ya da hayvan dostlarına taşı.
- Karakterleri ve süsleri sürükle. Çiçek, piknik ve süs konumları kaydedilir; hayvan konumları oturumluk kalır.
- Yetişkin alanında dil, ses, azaltılmış hareket, bilgi ve onaylı sıfırlama bulunur.

## Çıktılar

| Dosya | Kullanım |
|---|---|
| `Builds/Windows/YagmurBahcesi.exe` | Bu bilgisayarda oynanabilir önizleme |
| `Builds/Android/YagmurBahcesi-test.apk` | Android 8+ ARM64 test kurulumu; development modu kapalı, yerel debug imzalı |
| `Builds/Android/YagmurBahcesi-emulator.apk` | x86_64 Android emülatör testi; development modu açık |
| `YagmurBahcesi/Assets/RainGarden/Scenes/Garden.unity` | Gerçek Unity oyun sahnesi |
| `Artifacts/` | Gerçek çalıştırmalardan görüntüler ve test sonuçları |

APK dosyaları Play Store'a yüklenmiş değildir. Nihai mağaza kimliği, yayıncı bilgileri ve upload key henüz belirlenmedi.

## Geliştirme

PowerShell'de proje kökünden:

```powershell
.\tools\Invoke-Garden.ps1 -Action Open
.\tools\Invoke-Garden.ps1 -Action Check
.\tools\Invoke-Garden.ps1 -Action BuildWindows
.\tools\Invoke-Garden.ps1 -Action Smoke
.\tools\Invoke-Garden.ps1 -Action Smoke -Width 1280 -Height 960
.\tools\Invoke-Garden.ps1 -Action BuildAndroid
.\tools\Invoke-Garden.ps1 -Action BuildEmulator
.\tools\Test-AndroidPackage.ps1
```

Unity projesini kullanan başka bir Editor açıkken aynı proje üzerinde CLI derlemesi başlatma. Android derleme yolu Unicode kabul etmediği için script yalnızca Assets, Packages ve ProjectSettings'i `%USERPROFILE%/UnityBuilds/RainGardenAndroid` altına kopyalar; asıl proje taşınmaz. Bir dosyayı kaynak projeden bilerek sildiğinde, derleme kopyasındaki eski dosyayı da kontrollü olarak kaldırmak gerekir; eşitleme silme işlemi yapmaz.

Emülatör `%USERPROFILE%/UnityBuilds/AndroidSdk`, test AVD'si `%USERPROFILE%/UnityBuilds/Avd/RainGarden_API36.avd` altındadır. `tools/Start-AndroidPreview.ps1` ile açılabilir. `Test-AndroidGarden.ps1` yalnız bu özel test AVD'sindeki uygulama verisini sıfırlar; önce eski test kaydını Artifacts'a kopyalar. Normal oyun kaydına uygulanmaz.

Windows otomatik testleri ayrı geçici kayıt kullanır. Görüntü yakalamak için test sırasında kısa süreli görünür pencere açılır. Oyun ilerlemesi işletim sisteminin uygulama veri alanında JSON olarak tutulur; depodaki testler kişisel bilgi içermez.

## Belgeler

- [Son durum ve doğrulanmış sonuçlar](docs/GELISTIRME_DURUMU.md)
- [Oyun tasarımı](docs/GDD.md)
- [Kapsam genişletme ve tekrar oynanabilirlik önerisi — henüz uygulanmadı](docs/KAPSAM_GENISLETME_ONERISI.md)
- [Test planı](docs/TEST_PLANI.md)
- [Yayın hazırlığı ve dış bağımlılıklar](docs/YAYIN_HAZIRLIGI.md)
- [Pazar araştırması ve konsept seçimi](ARASTIRMA_VE_KONSEPT_ONAYI.md)

Sanat bu proje için yapay zekâ ile üretildi; prompt kayıtları `docs/ART_PROMPTS.json` ve `docs/ICON_PROMPT.txt` dosyalarındadır. Nunito fontunun lisansı `Assets/RainGarden/Fonts/OFL.txt` içindedir. Sesler proje koduyla üretildi.
