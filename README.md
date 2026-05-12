# 🏠 Haushaltsplaner – C# WPF Anwendung

Ein moderner Haushaltsplaner zur Verwaltung von Einnahmen und Ausgaben mit grafischer Auswertung, integriertem Rechner sowie Export- und Druckfunktionen.

Die Anwendung wurde mit **C#**, **WPF** und **OxyPlot** entwickelt und dient als Lern- und Entwicklungsprojekt für strukturierte Desktop-Programmierung.

---

## ✨ Projektübersicht

Der Haushaltsplaner unterstützt Nutzer dabei, ihre persönlichen Finanzen übersichtlich zu erfassen und auszuwerten.

Einnahmen und Ausgaben können manuell eingegeben, berechnet und grafisch dargestellt werden. Zusätzlich bietet die Anwendung einen integrierten Rechner sowie einfache Exportmöglichkeiten.

Der Fokus liegt auf einer modernen Benutzeroberfläche, verständlicher Bedienung und einer erweiterbaren Programmstruktur.

---

## 🎯 Ziele des Projekts

- Überblick über Einnahmen, Ausgaben und Saldo schaffen
- Finanzdaten übersichtlich erfassen
- Einnahmen und Ausgaben grafisch darstellen
- Einen einfachen Haushaltsrechner integrieren
- Daten als CSV-Datei speichern
- Haushaltsübersicht drucken
- Saubere und erweiterbare C#-Architektur aufbauen

---

## 🧩 Hauptfunktionen

### 💰 Einnahmenverwaltung

- Hinzufügen neuer Einnahmen
- Bearbeiten von Bezeichnung und Betrag
- Löschen einzelner Einträge
- Automatische Berechnung der Gesamteinnahmen
- Unterstützung des deutschen Zahlenformats mit Komma

---

### 💸 Ausgabenverwaltung

- Hinzufügen neuer Ausgaben
- Bearbeiten von Bezeichnung und Betrag
- Löschen einzelner Einträge
- Automatische Berechnung der Gesamtausgaben
- Übersichtliche Eingabemaske

---

### 📊 Übersicht

Die Startseite zeigt die wichtigsten Kennzahlen:

- Gesamteinnahmen
- Gesamtausgaben
- Aktueller Saldo

Der Saldo wird farblich hervorgehoben, damit positive und negative Werte schnell erkennbar sind.

---

### 📈 Diagramme mit OxyPlot

Die Anwendung verwendet **OxyPlot** zur grafischen Darstellung der Haushaltsdaten.

Aktuell umgesetzt:

- Balkendiagramm für Einnahmen, Ausgaben und Saldo
- Separate Diagrammansicht
- Aktualisieren des Diagramms per Button
- Zurücksetzen des Diagramms
- Dunkles Design passend zur Benutzeroberfläche

---

### 🧮 Integrierter Rechner

Die Anwendung enthält einen einfachen Taschenrechner mit folgenden Funktionen:

- Addition
- Subtraktion
- Multiplikation
- Division
- Prozentrechnung
- Vorzeichenwechsel
- Löschen einzelner Eingaben
- Zurücksetzen mit AC

---

### 💾 CSV-Export

Die eingegebenen Einnahmen und Ausgaben können als CSV-Datei gespeichert werden.

Exportierte Daten:

- Typ des Eintrags
- Bezeichnung
- Betrag
- Gesamtsaldo

---

### 🖨️ Druckfunktion

Die Anwendung kann eine einfache Haushaltsübersicht drucken.

Gedruckt werden:

- Einnahmenliste
- Ausgabenliste
- Gesamteinnahmen
- Gesamtausgaben
- Saldo

---

## 🛠️ Verwendete Technologien

- C#
- WPF / XAML
- .NET Desktop
- OxyPlot
- ObservableCollection
- Data Binding
- CSV-Dateiverarbeitung
- WPF PrintDialog / FlowDocument

---

## 🖥️ Benutzeroberfläche

Die Benutzeroberfläche besitzt ein modernes dunkles Design mit einer festen Seitenleiste.

Bereiche der Anwendung:

- Übersicht
- Ausgaben
- Einnahmen
- Diagramm
- Rechner
- Drucken
- CSV speichern

Das Layout ist klar strukturiert und für eine einfache Bedienung ausgelegt.

---

## 📂 Aktuelle Projektstruktur

```text
WPF_Test/
│
├── MainWindow.xaml
├── MainWindow.xaml.cs
├── pictures/
│   └── finance.png
└── README.md