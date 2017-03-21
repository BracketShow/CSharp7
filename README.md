# Quoi de neuf dans C# 7.0
Les nouvelles fonctionnalités de C# 7.0 expliquées et commentées
## Mise en contexte
Le 7 Mars 2017 Microsoft lançait Visual Studio 2017 et du même coup C# 7.0. Depuis que Microsoft a complètement réécrit le compilateur C# en C# de nouvelles fonctionnalités ne cessent d’être ajouté au langage. Le virage Agile que les groupe de produits de Microsoft ont adopté contribuent également à cette effervescence. De plus, Microsoft code maintenant dans l’open source. Vous pouvez d’ailleurs suivre le projet sur GitHub et même qui sait, y contribuer.

C# 6.0 nous avais déjà apporté son lot de nouveautés mais C# 7.0 pousse la note encore plus loin. 

J’aimerais vous présenter les nouveautés de C# 7.0 et mon opinion sur leur utilisation.

## Les variables de sortie (out variables)

Pour commencer, j’aimerais vous dire que dans la plupart des cas, c’est une mauvaise pratique que d’utiliser des variables out dans vos méthodes. Il existe cependant un cas de figure classique où il est approprié de les utiliser. Il s’agit du _try pattern_. Dans ce patron la méthode retourne un booléen pour annoncer la réussite de l’action et elle retourne le résultat du traitement dans une variable out. Le plus classique des cas est dans les méthodes _TryParse_ des différents types du _Framework_. 

```c#
int index;
if (Int32.TryParse("1", out index))
{
	Console.WriteLine($"Index is {index}");
}
```

Avant C# 7.0 nous devions absolument déclarer une variable pour contenir le résultat du paramètre *out*. Maintenant nous pouvons déclarer la variable au moment de sont passage en paramètre.

```c#
if (Int32.TryParse("1", out int index))
{
    Console.WriteLine($"Index is {index}");
}
```

En plus d'éviter de faire une déclaration sur une ligne sans assigner de valeur à la variable, cette méthode permet de restreindre la portée de la variable. En effet, la variable étant déclaré à l'intérieur de la clause conditionnelle elle a une portée définie uniquement à l'intérieur de celle-ci. Dès la sortie du bloc de code associé à la condition, la variable est relâchée. 

## Le _pattern matching_

La notion de _patterns_ introduit dans C# 7.0 est une syntaxe qui permet non seulement de tester pour si une variable est d'un certain type mais également pour en extraire la valeur.

Avant C# 7.0 nous aurions écrit :

```c#
object o = 1;
if (o is int)
{
    var i = (int)o;
    WriteLine($"o is int = {i}");
}
```

Maintenant nous pouvons écrire : 

```c#
object o = 1;
if (o is int i) WriteLine($"o is int = {i}");
```

Le _pattern matching_ fonctionne aussi dans les _switch case_. Je tiens à vous mentionner que je n'approuve généralement pas cette pratique. Il est habituellement déconseiller de faire un _switch case_ sur un type. Je vous présente cette fonctionnalité mais, S.V.P. n'allez pas mettre des _switch case_ partout et surtout pas sur des types. Il y plus souvent qu'autrement de meilleures façon de régler ce genre de problème.

Donc voici un exemple d'utilisation :

```c#
switch (shape)
{
    case Circle c:
        WriteLine($"circle with radius {c.Radius}, Area: {c.Area()}");
        break;
    case Rectangle r when (r.Length == r.Height):
        WriteLine($"{r.Length} x {r.Height} square, Area: {r.Area()}");
        break;
    case Rectangle r:
        WriteLine($"{r.Length} x {r.Height} rectangle, Area: {r.Area()}");
        break;
    default:
        WriteLine("<unknown shape>");
        break;
    case null:
        throw new ArgumentNullException(nameof(shape));
}
```

Choses importantes à remarquer :

1. il est possible d'ajouter une clause _when_ pour raffiner encore plus le matching en fonction de propriété de l'objet.
2. l'ordre des clause a de l'importance. Elle seront évaluées dans l'ordre du code.
3. La clause _default_ sera toujours évaluée en dernier peu import son emplacement. 
4. Le _case null_ peut être utiliser pour attribuer un traitement spécial si le case est _null_. S'il n'est pas gérer, il sera traité par la clause _default_.
5. Tou comme pour les conditions, les déclaration de variables définies dans les _case_ auront une portée limité à celui-ci. C'est pourquoi dans l'exemple, les deux variable _r_ n'entrent pas en conflit.

## Les _Tuples_

La classe ```System.Tuple<...>``` existe déjà depuis un bon moment dans le Framework mais son utilisation n'est pas très explicite. L'objet contient des propriétés typées, naturellement, mais pas nommées. Pour accéder aux valeur il faut utiliser les propriétés _Item1_, _Item2_, ...

La solution à ce problème est généralement réglé en créant une classe pour contenir les propriété en question. Par exemple :

```c#
class ActionResult
{
    public bool IsFailed { get; set; }
    public string Message { get; set; }
}
```

Cette classe n'as pas vraiment beaucoup de valeur. À part être utilisée comme conteneur de donnée dans l'exemple suivant :

```c#
static ActionResult ProcessOld()
{
    return new ActionResult()
    {
        IsFailed = true,
        Message = "Not Found"
    };
}
```

L'ajout des _Tuples_ en tant que citoyen de premier ordre dans C# 7.0 permet de simplifier le code tout en restant explicite tant au niveau du type de données que du nom. Nous pouvons maintenant écrire :

```c#
static (bool IsFailed, string Message) ProcesNew()
{
    return (true, "Not Found");
}
```

Cette méthode déclare plusieurs valeur de retour typées et nommées. Il y a plusieurs façon d'appeler cette méthode pour en recevoir le contenu.

Un première méthode est de déclarer une variable implicitement typée. Comme cette technique crée un ```System.Tuple``` il est possible d'utiliser les propriétés _Item1_ et _Item2_ mais C# 7.0 permet également d'utiliser les propriétés _IsFailed_ et _Message_.  

```c#
var newResult = ProcesNew();
if (newResult.Item1) WriteLine(newResult.Item2); // tuple standard
if (newResult.IsFailed) WriteLine(newResult.Message); // named properties
```

Il est également possible de créer une déclaration explicit des types de retour. Cette méthode a l'inconvénient de ne plus permettre d'utiliser les noms de propriété définis par la méthode. Seul les propriétés _Item1_ et _Item2_ sont disponibles. 

```c#
(bool, string) explicitResult = ProcesNew();
if (explicitResult.Item1) WriteLine(explicitResult.Item2);
```

Par contre, il est possible de créer une déclaration explicit et de renommer les membres du _Tuple_.

```c#
(bool HasFailed, string Reason) renamed = ProcesNew();
if (renamed.HasFailed) WriteLine(renamed.Reason);
```

Une autre fonctionnalité permet de faire les choses différemment. En utilisant la déconstruction d'objet, que nous verrons plus tard, nous pouvons assigner le retour à des variable locale directement.

```c#
(bool hasFailed, string reason) = ProcesNew();
if (hasFailed) WriteLine(reason);
```

On peut même le faire avec une déclaration implicite si on veut pas avoir à définir les types manuellement.

```c#
var (HasFailed, Reason) = ProcesNew();
if (HasFailed) WriteLine(Reason);
```

Comme vous pouvez le constater, les _Tuples_ offrent pleins de possibilités. Pour l'instant je ne suis pas certains quelle méthode je préfère. Par conséquent, je vous invite à être prudent en utilisant ces nouvelle fonctionnalités. Soyez certains que vous aurez des commentaires de vos collègues de travail qui se demanderons ce que vous essayer de faire avec ça.

> **NOTE:** Pour bénéficier des _tuples_ vou devez ajouter le package NuGet ```System.ValueTuple```

## La déconstruction d'objets

