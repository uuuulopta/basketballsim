
# Simulacija
Svaki tim po meču ima nasumično između 90-100 šuteva

Šansa da tim ostvari pogodak na svkom šutu jednaka je formuli: $$P = 0.3 + FP + FT + FR$$



Gde je:
- FP - Faktor forme protiv protivnika
- FT - Faktor totalne forme 
- FR - Faktor Razlike u FIBA rangu

$FP = L(\text{Broj pobeda protiv protivnika}- \text{Broj poraza protiv protvnika}) \quad[0,0.25]$

$FP = L(\text{Ukupan broj pobeda} - \text{Ukupan broj poraza}) \quad[0,0.25]$

$FR=\frac{(\text{FibaRang}-\text{ProtivnikovFibaRang})}{100}\quad[0,0.2]$

L - Je logistička funkcija. To je funkcija koja se sa povećanjem x, približava nekoj vrednosti (u ovom slučaju y)

$$L(x) = \frac{l}{1 + e^{-k(x-x_0)}}$$ 

Za simulaciju je izabrano:
- $l = 0.25$
- $k = 0.5$
- $x_{0}= -4$

| x   | L(x)  |
| --- | ----- |
| 1   | 0.045 |
| 2   | 0.065 |
| 5   | 0.15  |
| 8   | 0.20  |
| 10  | 0.23  |
