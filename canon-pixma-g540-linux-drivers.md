# Canon PIXMA G540 — Linux Driver Support

> Полное исследование: драйверы, расширенные функции, типы бумаг, ICC-профили, сравнение с Windows

---

## 1. Общая картина

Canon PIXMA G540 — односторонний (print-only) струйный принтер серии MegaTank с 6 цветами. В разных регионах называется G540, G550 (со сканером), G650 (G620 в США). Использует один и тот же печатный движок G500/G600 series.

**Официальная страница загрузки драйверов:**

- [Canon Europe — PIXMA G540](https://www.canon-europe.com/support/consumer/products/printers/pixma/g-series/pixma-g540.html?type=drivers&os=linux)
- [Canon Middle East — PIXMA G540](https://en.canon-me.com/support/consumer/products/printers/pixma/g-series/pixma-g540.html)
- [Canon UK — PIXMA G540](https://www.canon.co.uk/support/consumer/products/printers/pixma/g-series/pixma-g540.html)

---

## 2. Способы установки драйверов

### Ubuntu / Debian / Mint — через PPA (рекомендуемый)

```bash
sudo add-apt-repository ppa:thierry-f/fork-michael-gruz
sudo apt update
sudo apt install cnijfilter2
```

PPA поддерживает: Ubuntu 20.04 / 22.04 / 24.04 / 26.04.

> PPA поддерживает Thierry Ordissimo. Содержит официальные драйверы Canon: cnijfilter2, scangearmp2, UFRII.

### Arch / Manjaro — через AUR

```bash
yay -S cnijfilter2       # версия 6.81-1
```

### Ручная установка (любой дистрибутив)

1. Скачать `cnijfilter2-*-deb.tar.gz` (или `*-rpm.tar.gz`) с сайта Canon
2. Распаковать: `tar -xvzf cnijfilter2-*.tar.gz`
3. Запустить: `cd cnijfilter2-* && sudo ./install.sh`

### Driverless-печать (IPP Everywhere / AirPrint)

G540 поддерживает AirPrint и IPP Everywhere. Современные дистрибутивы автоматически находят принтер.

```bash
# Отключить driverless для перехода на драйвер Canon:
sudo apt purge ipp-usb cups-browsed
```

> **Минус driverless**: только базовая печать, без мониторинга чернил и обслуживания.

---

## 3. Расширенные функции обслуживания (аналог Windows)

Вопреки распространённому мнению, официальный драйвер Canon для Linux **включает утилиты обслуживания**:

| Функция | Утилита в Linux |
|---------|----------------|
| **Уровень чернил** | `cngpijmon[модель]` — GUI-монитор статуса |
| **Прочистка головок** | `cngpij` — Maintenance Utility |
| **Глубокая прочистка** | `cngpij` |
| **Проверка сопел** | `cngpij` |
| **Выравнивание головок** | `cngpij` |
| **Прочистка роликов** | `cngpij` |
| **Прочистка нижней пластины** | `cngpij` |
| **Тихий режим / Автоотключение** | `cngpij` |

Эти утилиты поставляются в составе пакета `cnijfilter2`. После установки доступны из терминала.

---

## 4. Что НЕ доступно в Linux

В отличие от Windows, отсутствуют:

- **Quick Menu** — меню быстрого доступа Canon
- **My Image Garden** — приложение для работы с фото
- **Easy-PhotoPrint Editor** — редактор фотопечати
- **IJ Printer Assistant Tool** — ассистент принтера (частично заменяется `cngpij` / `cngpijmon`)
- **Автоматические обновления драйверов**

---

## 5. Выбор бумаги (Media Type)

Официальный драйвер `cnijfilter2` в Linux предоставляет следующие типы бумаги (из PPD-файла `canon-g500.ppd`):

| Тип бумаги | Параметр PPD | Код Canon |
|-----------|-------------|-----------|
| Plain Paper | `plain` | — |
| Photo Paper Plus Glossy II | `glossygold` | PP-201/PP-208/PP-301 |
| Photo Paper Pro Luster | `luster` | LU-101 |
| Photo Paper Plus Semi-gloss | `semigloss` | SG-201 |
| Glossy Photo Paper "Everyday Use" | `glossypaper` | GP-501/GP-508 |
| Matte Photo Paper | `matte` | MP-101 |
| Photo Paper | `photopaper` | — |
| Envelope | `envelope` | — |
| Ink Jet Hagaki | `ijpostcard` | — |
| Hagaki | `postcard` | — |
| High Resolution Paper | `highres` | HR-101N |
| Inkjet Greeting Card | `greetingcard` | — |
| Card Stock | `cardstock` | — |

### Где выбрать

1. **GTK Print Dialog** (GIMP, LibreOffice, Evince) — вкладка **Page Setup** → **Media Type**
2. **Веб-интерфейс CUPS**: `http://localhost:631` → Printers → G540 → **Set Default Options**
3. **Командная строка**:

```bash
lp -d G540 -o MediaType=glossygold file.pdf
lpoptions -p G540 -l MediaType    # посмотреть доступные типы
```

---

## 6. Типы бумаг: сравнение Windows vs Linux

| Бумага | Windows G600 driver | Linux G500 PPD | Linux G600 PPD | Linux G5000 PPD |
|--------|---------------------|----------------|----------------|-----------------|
| Plain Paper | ✅ | ✅ | ✅ | ✅ |
| Photo Paper Plus Glossy II (PP-201) | ✅ | ✅ | ✅ | ✅ |
| Photo Paper Pro Luster (LU-101) | ✅ | ✅ | ✅ | ✅ |
| Photo Paper Plus Semi-gloss (SG-201) | ✅ | ✅ | ✅ | ✅ |
| Glossy Photo Paper (GP-501) | ✅ | ✅ | ✅ | ✅ |
| Matte Photo Paper (MP-101) | ✅ | ✅ | ✅ | ✅ |
| High Resolution Paper (HR-101N) | ✅ | ✅ | ✅ | ✅ |
| Photo Paper Pro Platinum (PT-101) | ✅ | ❌ | ❌ | ✅ |
| **Premium Fine Art Rough (FA-RG1)** | ✅ | ❌ | ❌ | ❌ |
| Restickable Photo Paper (RP-101) | ✅ | ❌ | ❌ | ❌ |
| Double sided Matte Paper (MP-101D) | ✅ | ❌ | ❌ | ❌ |
| Inkjet Greeting Card | ❌ | ✅ | ✅ | ❌ |
| Card Stock | ❌ | ✅ | ✅ | ❌ |

> **Ключевое**: «Premium Fine Art Rough» (FA-RG1, 320 gsm) — **отсутствует во всех Linux PPD**.

---

## 7. Premium Fine Art Rough (FA-RG1) на Linux

### Почему нет?

- **Закрытый бинарник `cif`**: фильтр рендеринга Canon — closed source. PPD передаёт строку `MediaType(xxx)` → бинарник `cif`, который должен её распознать. Строки, не известные `cif`, игнорируются или вызывают ошибку.
- **Canon не включил Fine Art бумаги в Linux PPD для G-серии** — это осознанное решение, не баг.
- В старой кодовой базе `cnijfilter-common` (для MG-серии) были `fineartphotorag` и `otherfineart`, но не `fineartrough`.

### Успешные кейсы?

**Ни одного публичного отчёта** об успешной печати на FA-RG1 через Linux-драйвер для G540/G550/G650. Проверены: Reddit, DPReview, AskUbuntu, Linux Mint Forums, ArchWiki, GitHub.

На DPReview пользователи G540/G550/G650 обсуждают ICC-профили, но все на Windows/Mac. Про Linux + Fine Art — полная тишина.

### Что можно сделать?

#### Вариант 1: Matte Photo Paper + свой ICC (работает сейчас)

```bash
lp -d G540 -o MediaType=matte -o CNQuality=1 -o PageSize=A4 file.pdf
```

- ✅ Работает без танцев
- ⚠️ Не учитывает толщину 320 gsm — риск затирания головкой
- ⚠️ Без ICC-профиля под FA-RG1 цвета не точны

#### Вариант 2: Попробовать `otherphoto`

В PPD уже есть `otherphoto/Other Photo Paper` — универсальный тип для нестандартных фотобумаг:

```bash
lp -d G540 -o MediaType=otherphoto -o CNQuality=1 -o PageSize=A4 file.pdf
```

#### Вариант 3: Добавить MediaType в PPD (хак, маловероятно сработает)

```bash
# Бэкап
sudo cp /etc/cups/ppd/Canon_G500_series.ppd /etc/cups/ppd/Canon_G500_series.ppd.bak

# Добавить строку
sudo sh -c 'echo "*MediaType fineartrough/Premium Fine Art Rough: \"<</MediaType(fineartrough)>>setpagedevice\"" >> /etc/cups/ppd/Canon_G500_series.ppd'

# Перезапустить CUPS
sudo systemctl restart cups
```

⚠️ **Скорее всего НЕ сработает**: бинарник `cif` не знает строку `fineartrough`.

#### Вариант 4: Виртуалка с Windows

Для ответственной печати на FA-RG1 — проброс USB в Windows VM. Единственный способ получить 100% функционал драйвера, включая:

- MediaType = Premium Fine Art Rough
- PageSize = XXX (Art Paper Margin 35) — отступ 35 мм для толстой бумаги
- Полную цветокоррекцию

---

## 8. Качество печати (Print Quality)

Доступные уровни (из PPD):

```bash
lpoptions -p G540 -l CNQuality
# CNQuality/Print Quality: 1(High) 2 3 4(Fast)
```

| Уровень | Описание |
|---------|----------|
| **1 — High** | Наивысшее качество, медленно |
| **2–3 — Standard** | Сбалансированное |
| **4 — Fast** | Черновик |

---

## 9. Цветокоррекция и ICC-профили

### Что есть в Linux (cnijfilter2)

PPD-файл предоставляет:

- **Color Model**: RGB
- **Manual Color Adjustment** — ручная коррекция:
  - Gamma (1.4 / 1.8 / 2.2)
  - Cyan, Magenta, Yellow — раздельные ползунки
  - Brightness / Contrast
- **Color Correction**: можно отключить (`None`) — тогда за цвет отвечает приложение

### Что ОТСУТСТВУЕТ (и в Windows тоже!)

Canon **не поставляет ICC-профили для G540/G550/G650** — ни под Windows, ни под Linux. Единственный ICC в комплекте — generic-профиль от 2015 года.

> *«К моему удивлению, для стандартных бумаг вроде GP-501, PP-201, PT-101 нет ICC-профилей. Единственный ICC, идущий с драйвером — какой-то generic от 2015»*
> — пользователь orlovsn на DPReview

### Как работать с ICC в Linux

#### Печать через GIMP (правильный colour-managed workflow)

```
1. File → Print → Color Management
2. Color Handling: "Separate Cyan, Magenta, Yellow, Black"
3. Printer Profile: [выбрать ICC-профиль для бумаги]
4. Printer Settings → Media Type = [тип бумаги]
5. Printer Settings → Color Correction = None
```

#### Печать через Darktable

```
1. Print → Printer Profile = [ICC для бумаги]
2. Intent = Perceptual / Relative Colorimetric
3. В настройках принтера: Media Type = [бумага], Color Correction = None
```

#### Создание своего ICC-профиля (ArgyllCMS)

```bash
# 1. Генерируем тестовую мишень
targen -v -d2 -f480 myprofile
printtarg -v -ii1 -pA4 myprofile

# 2. Печатаем мишень с ОТКЛЮЧЁННОЙ цветокоррекцией
lp -d G540 -o MediaType=glossygold -o CNColorAdjust=False myprofile.ps

# 3. Сканируем и строим профиль
chartread myprofile
colprof -v -A"Canon" -M"PIXMA G540" -D"PP-201 Glossy" -qh -cmt -S sRGB.icc myprofile
```

#### Готовые ICC-профили

- [Marrutt — бесплатные ICC для Canon G650](https://marrutt.com/help-support/icc-profiles/generic-icc-printer-profiles-for-canon-pixma-g650/)
- Пользователи на [DPReview](https://www.dpreview.com/forums/thread/4614299) делятся своими профилями

---

## 10. TurboPrint — коммерческая альтернатива

| | cnijfilter2 (Canon) | TurboPrint |
|---|---|---|
| Бумаги (Media Types) | ✅ Все основные | ✅ Все + Fine Art |
| ICC-профили | ❌ Нет в комплекте | ✅ Часть в комплекте |
| Цветокоррекция | Ручная (Gamma/CMY) | ✅ Авто + ручная |
| Стоимость | Бесплатно | ~€34 |
| Поддержка G540 | ✅ | ❓ Не подтверждена |

> TurboPrint для G540 не подтверждён в списке совместимости, но G-серия (G500/G600/G5000/G6000) у них поддерживается. Можно попробовать триал: [turboprint.info](https://www.turboprint.info)

---

## 11. Альтернативные драйверы

### Gutenprint

```bash
sudo apt install printer-driver-gutenprint
```

- ❌ Поддержка G540 не подтверждена
- ⚠️ Качество печати хуже, чем cnijfilter2/TurboPrint (по отзывам — «washout and yellowish»)
- ✅ Бесплатно, open source

---

## 12. Сводная таблица: Linux vs Windows

| Функция | Linux (cnijfilter2) | Windows |
|---------|---------------------|---------|
| **Выбор бумаги** | ✅ Основные | ✅ Все (включая Fine Art) |
| **Premium Fine Art Rough (FA-RG1)** | ❌ | ✅ |
| **Art Paper Margin 35** | ❌ | ✅ |
| **Качество печати** | ✅ High/Standard/Fast | ✅ |
| **Ручная цветокоррекция** | ✅ Gamma, CMY, яркость | ✅ |
| **ICC-профили Canon** | ❌ Нет (generic 2015) | ❌ Нет (то же самое) |
| **Custom ICC через приложения** | ✅ GIMP/Darktable | ✅ Photoshop и др. |
| **Мониторинг чернил** | ✅ `cngpijmon` | ✅ |
| **Прочистка головок** | ✅ `cngpij` | ✅ |
| **Проверка сопел** | ✅ `cngpij` | ✅ |
| **Выравнивание головок** | ✅ `cngpij` | ✅ |
| **Quick Menu / My Image Garden** | ❌ | ✅ |
| **Auto Power / Quiet Mode** | ✅ `cngpij` | ✅ |

---

## 13. Выводы

1. **Canon PIXMA G540 имеет хорошую базовую поддержку в Linux** — печать, обслуживание, мониторинг чернил работают.

2. **Главный пробел** — отсутствие Fine Art бумаг (Premium Fine Art Rough FA-RG1) в Linux PPD. Это касается и Pro Platinum (PT-101) в младших PPD (G500/G600).

3. **Для повседневной печати** Linux-драйвера достаточно. Для художественной печати на FA-RG1 с правильными полями (Art Paper Margin 35) нужен либо Windows (физически или VM), либо TurboPrint (если подтвердится поддержка G540).

4. **ICC-профили** — слабое место и в Windows, и в Linux. В обоих случаях нужны либо сторонние, либо самодельные через ArgyllCMS/ColorMunki/SpyderPrint.

---

## Ссылки

- [Canon Europe — Drivers PIXMA G540](https://www.canon-europe.com/support/consumer/products/printers/pixma/g-series/pixma-g540.html)
- [PPA thierry-f/fork-michael-gruz (Launchpad)](https://launchpad.net/~thierry-f/+archive/ubuntu/fork-michael-gruz)
- [AUR cnijfilter2](https://aur.archlinux.org/packages/cnijfilter2)
- [UbuntuHandbook — Canon Printer Driver Guide](https://ubuntuhandbook.org/index.php/2020/05/canon-printer-scangear-mp-ubuntu-20-04/)
- [Easy Linux Tips — Canon PIXMA Installation](https://easylinuxtipsproject.blogspot.com/p/canon-printers.html)
- [DPReview — ICC profiles for G540/G550/G650](https://www.dpreview.com/forums/thread/4614299)
- [DPReview — Share ICC profiles for G650/G540](https://www.dpreview.com/forums/thread/4603478)
- [Marrutt — Free ICC profiles for Canon G650](https://marrutt.com/help-support/icc-profiles/generic-icc-printer-profiles-for-canon-pixma-g650/)
- [TurboPrint — Commercial Linux Printer Driver](https://www.turboprint.info)
- [Gutenprint — OpenPrinting](https://www.openprinting.org/driver/gutenprint)
- [GitHub — endlessm/cnijfilter2](https://github.com/endlessm/cnijfilter2)
- [GitHub — matthiasbock/canon-pixma-printer-driver](https://github.com/matthiasbock/canon-pixma-printer-driver)
- [Linux Wiki — Getting Canon PIXMA to work on Linux](https://linux.fandom.com/wiki/Getting_Canon_PIXMA_to_work_on_Linux)
