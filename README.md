# Quoi de neuf dans C# 7.0
Les nouvelles fonctionnalités de C# 7.0 expliquées et commentées
## Mise en contexte
Le 7 Mars 2017 Microsoft lançait Visual Studio 2017 et du même coup C# 7.0. Depuis que Microsoft a complètement réécrit le compilateur C# en C# de nouvelles fonctionnalités ne cessent d’être ajouté au langage. Le virage Agile que les groupe de produits de Microsoft ont adopté contribuent également à cette effervescence. De plus, Microsoft code maintenant dans l’open source. Vous pouvez d’ailleurs suivre le projet sur GitHub et même qui sait, y contribuer.

C# 6.0 nous avais déjà apporté son lot de nouveautés mais C# 7.0 pousse la note encore plus loin. 

J’aimerais vous présenter les nouveautés de C# 7.0 et mon opinion sur leur utilisation.

## Les variables de sortie (out variables)

Pour commencer, j’aimerais vous dire que dans la plupart des cas, c’est une mauvaise pratique que d’utiliser des variables out dans vos méthodes. Il existe cependant un cas de figure classique où il est approprié de les utiliser. Il s’agit du _try pattern_. Dans ce patron la méthode retourne un booléen pour annoncer la réussite de l’action et elle retourne le résultat du traitement dans une variable out. Le plus classique des cas est dans les méthodes _TryParse_ des différents types du _Framework_. 

```csharp
int index;
if (Int32.TryParse("1", out index))
{
	Console.WriteLine($"Index is {index}");
}
```

Avant C# 7.0 nous devions absolument déclarer une variable pour contenir le résultat du paramètre *out*. Maintenant nous pouvons déclarer la variable au moment de sont passage en paramètre.

```csharp
if (Int32.TryParse("1", out int index))
{
    Console.WriteLine($"Index is {index}");
}
```

En plus d'éviter de faire une déclaration sur une ligne sans assigner de valeur à la variable, cette méthode permet de restreindre la portée de la variable. En effet, la variable étant déclaré à l'intérieur de la clause conditionnelle elle a une portée définie uniquement à l'intérieur de celle-ci. Dès la sortie du bloc de code associé à la condition, la variable est relâchée. 

## Le _pattern matching_

La notion de _patterns_ introduit dans C# 7.0 est une syntaxe qui permet non seulement de tester pour si une variable est d'un certain type mais également pour en extraire la valeur.

Avant C# 7.0 nous aurions écrit :

```csharp
object o = 1;
if (o is int)
{
    var i = (int)o;
    WriteLine($"o is int = {i}");
}
```

Maintenant nous pouvons écrire : 

```csharp
object o = 1;
if (o is int i) WriteLine($"o is int = {i}");
```

Le _pattern matching_ fonctionne aussi dans les _switch case_. Je tiens à vous mentionner que je n'approuve généralement pas cette pratique. Il est habituellement déconseiller de faire un _switch case_ sur un type. Je vous présente cette fonctionnalité mais, S.V.P. n'allez pas mettre des _switch case_ partout et surtout pas sur des types. Il y plus souvent qu'autrement de meilleures façon de régler ce genre de problème.

Donc voici un exemple d'utilisation :

```csharp
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

```csharp
class ActionResult
{
    public bool IsFailed { get; set; }
    public string Message { get; set; }
}
```

Cette classe n'as pas vraiment beaucoup de valeur. À part être utilisée comme conteneur de donnée dans l'exemple suivant :

```csharp
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

```csharp
static (bool IsFailed, string Message) ProcesNew()
{
    return (true, "Not Found");
}
```

Cette méthode déclare plusieurs valeur de retour typées et nommées. Il y a plusieurs façon d'appeler cette méthode pour en recevoir le contenu.

Un première méthode est de déclarer une variable implicitement typée. Comme cette technique crée un ```System.Tuple``` il est possible d'utiliser les propriétés _Item1_ et _Item2_ mais C# 7.0 permet également d'utiliser les propriétés _IsFailed_ et _Message_.  

```csharp
var newResult = ProcesNew();
if (newResult.Item1) WriteLine(newResult.Item2); // tuple standard
if (newResult.IsFailed) WriteLine(newResult.Message); // named properties
```

Il est également possible de créer une déclaration explicit des types de retour. Cette méthode a l'inconvénient de ne plus permettre d'utiliser les noms de propriété définis par la méthode. Seul les propriétés _Item1_ et _Item2_ sont disponibles. 

```csharp
(bool, string) explicitResult = ProcesNew();
if (explicitResult.Item1) WriteLine(explicitResult.Item2);
```

Par contre, il est possible de créer une déclaration explicit et de renommer les membres du _Tuple_.

```csharp
(bool HasFailed, string Reason) renamed = ProcesNew();
if (renamed.HasFailed) WriteLine(renamed.Reason);
```

Une autre fonctionnalité permet de faire les choses différemment. En utilisant la déconstruction d'objet, que nous verrons plus tard, nous pouvons assigner le retour à des variable locale directement.

```csharp
(bool hasFailed, string reason) = ProcesNew();
if (hasFailed) WriteLine(reason);
```

On peut même le faire avec une déclaration implicite si on veut pas avoir à définir les types manuellement.

```csharp
var (HasFailed, Reason) = ProcesNew();
if (HasFailed) WriteLine(Reason);
```

Comme vous pouvez le constater, les _Tuples_ offrent pleins de possibilités. Pour l'instant je ne suis pas certains quelle méthode je préfère. Par conséquent, je vous invite à être prudent en utilisant ces nouvelle fonctionnalités. Soyez certains que vous aurez des commentaires de vos collègues de travail qui se demanderons ce que vous essayer de faire avec ça.

> **NOTE:** Pour bénéficier des _tuples_ vou devez ajouter le package NuGet ```System.ValueTuple```

## La déconstruction d'objets

La déconstruction d'objet de C# 7.0 permet de séparer un _tuple_ ou n'importe quel objet en ses constituants et les assigner à des variables individuelles.

La déconstruction peut prendre la forme d'une déclaration individuelle des variables:

```csharp
(string firstName, string _, string lastName) = person;
WriteLine($"{lastName}, {firstName} ({_})");
```

> Le caractère "_" est utilisé pour ignorer un des constituants de la déconstruction. Il peut aussi être utiliser comme une variable sans pour autant entrer en conflit.

L'utilisation de _var_ peut éviter de nommer les types de retour. Cette  déclaration peut être utiliser de deux façon différente. À l'intérieur de la déclaration:

```csharp
(var firstName, var _, var lastName) = person;
WriteLine($"{lastName}, {firstName} ({_})");
```

ou à l'extérieur:

```csharp
var (firstName, _, lastName) = person;
WriteLine($"{lastName}, {firstName} ({_})");
```

On peut évidemment aussi assigner lae résultat de la déconstruction à des variable existante:

```csharp
string firstName, middleName, lastName;
(firstName, middleName, lastName) = person;
WriteLine($"{lastName}, {firstName} ({_})");
```

Tout ça est bien beau mais il faut quand même définir comment faire la déconstruction d'un objet. Pour ce faire, vous devez ajouter une méthode nommé _Desconstructor_. Cette méthode ne dois que déclarer des paramètres _out_. 

```csharp
public class Person
{
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }

    public Person(string firstName, string middleName, string lastName)
    {
        FirstName = firstName;
        MiddleName = middleName;
        LastName = lastName;
    }

    public void Deconstruct(out string firstName, out string middleName, out string lastName)
    {
        firstName = FirstName;
        middleName = MiddleName;
        lastName = LastName;
    }
}
```

Il est commun dans ce cas d'avoir un symétrie entre la construction et la déconstruction de l'objet.

## Les fonctions locale

Parfois il serait utile de pouvoir définir une fonction d'aide pour simplifier le code. Mais avant C# 7.0 la seule option était de définir une fonction privé, qui sera visible d'ailleurs dans la classe, ou une expression lambda qui peut ne pas être aussi explicit au niveau de la définition.

C# 7.0 permet de créer des fonction locale à même la porté d'une méthode. Cette fonction a accès aux variable déclaré à l'intérieur de sa méthode. Par exemple ici nous essayons de faire un simple tri:

```csharp
private static void Sort(int[] numbers)
{
    for(int i = 0; i < numbers.Length - 1; i++)
    {
        for (int j = i + 1; j < numbers.Length; j++)
        {
            if (numbers[i] > numbers[j])
            {
                var temp = numbers[j];
                numbers[j] = numbers[i];
                numbers[i] = temp;
            }
        }
    }
}
```

Pour éviter d'ajouter un commentaire pour expliquer ce qu'on essai de faire à l'intérieur de la condition nous pouvons créer une fonction locale pour exprimer qu'il s'agit d'un swap.

```csharp
private static void Sort(int[] numbers)
{
    for (int i = 0; i < numbers.Length - 1; i++)
    {
        for (int j = i + 1; j < numbers.Length; j++)
        {
            if (numbers[i] > numbers[j])
            {
                Swap(i, j);
            }
        }
    }

    void Swap(int a, int b)
    {
        var temp = numbers[b];
        numbers[b] = numbers[a];
        numbers[a] = temp;
    }
}
```

Comme la fonction _swap_ est définie au même niveau que le paramètre _number_ de la méthode _Sort_, elle a accès à son contenu directement sans avoir à le passer en paramètre. 

## De meilleures déclarations de valeur littérale

Le caractère "_" peut maintenant être utilisé à l'intérieur de n'importe quelle expression de valeur numérique pour améliorer la lisibilité nombres plus long. 

```csharp
var d = 1_234_567;
var x = 0xAA_BB_CC_DD_EE_FF;
```

Il peuvent être littéralement utilisé n'importe où (sauf au début et à la fin) et n'ont aucun effet sur la valeur.

```csharp
var d1 = 1__________2___3____4;
var d2 = 1234;
```

Ces deux valeurs sont identiques.

C# 7.0 introduit également une nouvelle déclaration littérale pour les nombres binaires.

```csharp
var b = 0b1010_1011_1100_1101_1110_1111;
```

## Les retour de fonction par référence (Ref returns)

Vous souvenez-vous de vos cours sur les pointeurs. En C# on a peut oublié que ça existait. On a appris qu'il y a des type référence et des types valeur et que c'est ce qui dictait l'effet d'un changement sur une valeur de l'un ou l'autre de ces types. 

Mais il semble que dans certaines situation particulière, les jeux par exemple, qu'il soit utile pour un fonction de retourner la référence à une valeur. Prenons par exemple le cas d'une méthode de recherche dans un _array_ qui retourne l'élément trouvé par référence.

```csharp
public static ref int Find(int number, int[] numbers)
{
    for (int i = 0; i < numbers.Length; i++)
    {
        if (numbers[i] == number)
        {
            return ref numbers[i]; // return the storage location, not the value
        }
    }
    throw new IndexOutOfRangeException($"{nameof(number)} not found");
}
```

Si on passe un _array_ a cette fonction on peut trouver l'élément et changer sa valeur en assignant une nouvelle valeur à l'élément retourné.

```csharp
int[] array = { 1, 15, -39, 0, 7, 14, -12 };
ref int found = ref Find(7, array); // aliases 7's place in the array
found = 9; // replaces 7 with 9 in the array
```

Après cette opération, la variable _array_ contient les éléments 1, 15, -39, 0, 9, 14, -12. L'assignation de la valeur 9 à l'élément trouvé remplace directement la case mémoire de l'_array_ à la position trouvée. 

## Les corps de membre sous forme d'expression

C# 6.0 a introduit le concept de corp de membre sous forme d'expression pour les méthode et les propriété. Ce fut un succès Microsoft a décidé d'ajouter la possibilité d'utiliser ce concept ailleurs. Nous pouvons maintenant le faire pour les accesseurs, les constructeurs et les finaliseurs.

```csharp
class Person
{
    private static ConcurrentDictionary<int, string> names = new ConcurrentDictionary<int, string>();
    private int id = names.Count;

    public Person(string name) => names.TryAdd(id, name); // constructors
    ~Person() => names.TryRemove(id, out _);              // destructors
    public string Name
    {
        get => names[id];                                 // getters
        set => names[id] = value;                         // setters
    }
}
```

## L'expression _throw_

Traditionnellement le mot clé _throw_ a toujours été traité comme un _statement_. Il est maintenant possible avec C# 7.0 de l'utiliser comme une expression.

```csharp
class Person
{
    public string Name { get; }
    public Person(string name) => Name = name ?? throw new ArgumentNullException(nameof(name));
    public string GetFirstName() => throw new NotImplementedException();
    public string GetLastName()
    {
        var parts = Name.Split(' ');
        return (parts.Length > 1) ? parts[1] : throw new InvalidOperationException("No space!");
    }
}
```

## Conclusion

C# 7.0 ajoute beaucoup de nouvelles fonctionnalités. Certaines sont bénéfique et d'autre sont pour le moins dangereuse. Je vous invite à explorer ces nouvelles fonctionnalités avec précaution. Elle peuvent vous faire économiser quelque ligne de code mais elle peuvent aussi vous faire appliquer de mauvaise pratique de programmation. 
