# Repository für das Unity-Projekt zum Thema Portale in VR
Ziel dieses Projekts war es, Portale als Fortbewegungsmethode in Unity innerhalb einer VR-Anwendung zu implementieren. Portale sind Objekte in einer Szene, mit denen Nutzer interagieren können, um sofort zu einem Zielort teleportiert zu werden. Zum Ausführen der Skripte wird das Package "Vive Input Utility" benötigt. Das Projekt wurde mit der Meta Quest 2 getestet.

## portale_start
Die Szene "portale_start" dient als Einstiegsszene und führt den Nutzer durch verschiedene einfache Modelle von Portalen. Der Nutzer kann durch beschriftete 3D-Buttons in der Szene zwischen den einzelnen Tests wechseln. 

### Portal-Pads
![Verbindung zweier Portal-Pads](https://github.com/wilhelm-f/AVR_Portale/blob/main/Bilder/portale_pads.png?raw=true)
  Der Nutzer startet bei einem Testaufbau, der aus zwei Pads auf dem Boden mit jeweils einem Button besteht. Diese Pads sind hierbei die Portale und sind miteinander verbunden. Wird ein Pad betreten, wechselt das Material von Eingangs- und Ausgangs-Pad und das Portal gilt als aktiv, die Änderung des Materials zeigt gleichzeitig an, wohin das Portal den Nutzer teleportieren wird. Die Teleportation kann durch Interaktion mit einem sich bei einem Pad befindenden Button eingeleitet werden. Die Interaktion mit dem Button erfolgt über die Trigger Taste.
Der nächste Testaufbau ist nahezu identisch zum ersten Aufbau, mit dem Unterschied, dass hier die Teleportation durch einen Button am Controller bestätigt wird. Hier wurde der A-Button der rechten Hand gewählt.

### Vertikale Portale
![Vertiakes Portal](https://github.com/wilhelm-f/AVR_Portale/blob/main/Bilder/portal_vertikal.png?raw=true)
  Im dritten Testaufbau findet der Nutzer einen neuen Ansatz von Portalen. Diese Portale sind vertikal und funktionieren wie Türen. Betritt der Nutzer ein Portal, erkennt das dazugehörige Skript eine Kollision zwischen Kamera und Portalfläche und der Nutzer wird ohne sonstigen Input zum Zielportal teleportiert. Hierbei wird die Blickrichtung nach der Teleportation entsprechend zum Ausgangsportal angepasst.
Im letzten Testaufbau dieser Szene sieht der Nutzer ein Eingangsportal und mehre weitere verschieden gefärbte Portale. Durch eine Konsole neben dem Eingangsportal kann das Ziel dieses Protals geändert werden, wobei das Material der Portalfläche des Eingangsportals sich automatisch dem des Ziels anpasst. Alle anderen Portale werden durch einen Wechsel deaktiviert.
Nach diesem Testaufbau kann der Nutzer in eine nächste Szene wechseln.

## portale_extended
In dieser Szene wurden die bisher bestehenden Ideen genutzt und erweitert.

### Preview-Protale
Im ersten Aufbau dieser Szene findet sich die Grundstruktur der vertikalen Portale wieder, hier wurde jedoch das transparente Material der Portalfläche durch eine Vorschau des Zielportals ausgetauscht. Jedes Portal beinhaltet eine Kamera, die von einem verbundenen Portal genutzt werden kann, um das Bild auf die jeweilige Portalfläche zu projizieren. Der Testaufbau beinhaltet außerdem zwei Bälle, mit denen der Nutzer interagieren kann. Durch das Drücken der Grip-Taste können diese aufgehoben, und durch ein Portal geworfen werden. Hierbei wird der Geschwindigkeitsvektor des Balls in Betracht auf die Richtung des Ausgangsportals angepasst. Der Ball kann in der Vorschau weiter verfolgt werden.

### Blickrichtung-Portale
![Vorschau eines "Blickrichtung-Portals"](https://github.com/wilhelm-f/AVR_Portale/blob/main/Bilder/portal_blickwinkel.png?raw=true)
  Der letzte Testaufbau verbindet die Idee von Preview-Portalen und Portal-Pads. Hierbei ist ein Portal mit Vorschau mit einem Portal-Pad verbunden. Die Vorschau zeigt hierbei das Pad in einer Top-Down Perspektive mit einem Richtungsindikator. Nähert sich der Nutzer dem Preview-Portal, so kann dieser durch den Stick des rechten Controllers eine Richtung wählen. Beim Betreten des Portals wird der Nutzer dann so teleportiert, dass dieser automatisch in die gewählte Blickrichtung sieht.

##
Alle Prefabs und Skripte werden in der Datei "Dokumentation.pdf" genauer beschrieben
