# TetrAI

Ce projet implémente une IA pour Tetris, basée sur un **algorithme génétique**.

---

## Le Tetris

Je ne suis pas reparti de zéro pour coder Tetris : j’ai utilisé le code de **Zigurous**, disponible sur [YouTube](https://www.youtube.com/watch?v=ODLzYI4d-J8) et [GitHub](https://github.com/zigurous/unity-tetris-tutorial).

Le code a été **légèrement modifié** afin d’optimiser le rendu et le temps de traitement.  
J’ai également retiré le *ghost piece* (la prévisualisation de l’arrivée de la pièce).

---

## Règles

- Le jeu utilise les **7 tetrominos classiques** :
  - I, J, L, S, Z, O, T
- Le plateau mesure **10 cases de largeur** et **20 cases de hauteur**.
- À chaque tour, une nouvelle pièce apparaît au sommet, centrée horizontalement.
- La pièce descend automatiquement à vitesse régulière, mais le joueur peut :
  - la déplacer latéralement (gauche/droite),
  - la faire descendre plus vite,
  - ou exécuter un **Hard Drop** (descente instantanée au plus bas possible).
- Lorsqu’une pièce ne peut plus descendre, elle est figée et une nouvelle pièce apparaît.
- Lorsqu’une ligne est complètement remplie, elle est supprimée et les pièces au-dessus descendent d’un cran.
- La partie se termine si une nouvelle pièce ne peut pas apparaître (plateau saturé).

---

## Objectif

L’objectif du joueur (ou de l’IA) est d’**éviter que les pièces atteignent le sommet du plateau** en complétant régulièrement des lignes.  

---

# L’IA

L’IA repose sur un **algorithme génétique**, qui s’inspire du processus d’évolution théorisé par **Darwin** (sélection, mutation, reproduction).

Chaque coup possible est évalué en fonction de **plusieurs critères pondérés**.  
Actuellement, l’IA utilise **4 paramètres principaux** pour évaluer un coup :

1. Le **nombre de lignes complétées** grâce au mouvement  
2. Le **nombre de trous** créés ou laissés après le coup  
3. La **hauteur maximale** atteinte par les blocs après le coup  
4. La **rugosité** du plateau (variation des hauteurs entre colonnes voisines)

Ces paramètres sont combinés dans une fonction de score.  
L’algorithme génétique ajuste progressivement les poids associés à chaque paramètre afin d’optimiser les performances de l’IA.

---

## Algorithme génétique

L’IA est entraînée à l’aide d’un algorithme génétique simple :  

1. **Population initiale** :  
   - On fait jouer **9 agents** (chacun avec un set de poids aléatoires).  
   
2. **Évaluation** :  
   - Chaque agent joue une partie de Tetris, son score final est enregistré.  

3. **Sélection** :  
   - On garde les **2 meilleurs agents** (ceux ayant obtenu les scores les plus élevés).  

4. **Reproduction (croisement)** :  
   - Les agents enfants héritent de leurs poids de la manière suivante :  
     - Pour chaque paramètre (lignes, trous, hauteur max, rugosité),  
       l’enfant prend la valeur soit du **parent 1**, soit du **parent 2** (au hasard).  

5. **Mutation** :  
   - Pour éviter la stagnation et encourager l’exploration,  
     chaque poids peut être **augmenté ou diminué de 0.1 aléatoirement**.  

6. **Nouvelle génération** :  
   - Les nouveaux agents remplacent l’ancienne population,  
     et le processus recommence depuis l’étape 2.  

Au fil des générations, l’algorithme ajuste les poids afin d’améliorer la performance de l’IA.

---

## Explication des paramètres

### 1. Nombre de lignes
Chaque ligne complétée rapporte un bonus. Plus l’IA peut en effectuer, plus le coup est valorisé.

### 2. Nombre de trous
Un trou correspond à une case vide sous une case remplie. Les trous sont négatifs car ils compliquent le remplissage futur du plateau.

### 3. Hauteur maximale
La hauteur du bloc le plus élevé est prise en compte. Plus cette hauteur est grande, plus le risque de perdre est élevé.

### 4. Rugosité
La rugosité mesure les différences de hauteur entre colonnes voisines. Plus elle est élevée, plus le plateau est accidenté et difficile à gérer.

---

## Paramètres supplémentaires possibles

En plus des 4 critères principaux, d’autres paramètres peuvent être intégrés pour affiner l’IA :

- Le **nombre de points** gagnés selon les règles classiques de Tetris (1 à 4 lignes, T-Spins, combos, etc.)  
- Le **nombre de puits** créés (espaces verticaux où seule une barre “I” peut entrer)  
- L’évaluation de si un trou nouvellement créé est **réparable** avec les pièces futures  
- La **stabilité** du plateau (éviter les colonnes isolées trop hautes)  
- La **parité** des colonnes (important pour certaines stratégies à long terme)

---

## Installation

1. Cloner le projet :
   ```bash
   git clone https://github.com/ton-pseudo/TetrAI.git
   ````
2. Ouvrir le dossier avec Unity (2021.3 LTS ou plus récent recommandé)
3. Lancer la scène principale : Scenes/MainScene.unity

---

## Utilisation

- Lancer le projet dans Unity.
- L'IA joue automatiquement les pièces.
- Les logs dans la consoles affichent les coups choisis et les scores évalués.
