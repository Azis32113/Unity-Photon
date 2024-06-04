using UnityEngine;

public static class Utils {
    public static Vector3 GetRandomPosition()
    {
        return new Vector3(Random.Range(-20, 20), 3, Random.Range(-20, 20));
    }

    public static string GetRandomName()
    {
        string[] names = {"Kayda", "Contreras", "Emilio", "Bass", "Zahra", "Blackburn", "Zahir", "Rivera", "Lillian", "Xiong", "Azrael", "Ramos", "Alice", "Boyer", "Zeke", "Lane", "Amy", "Cohen", "Killian", "Carter", "Lucy", "Garrison", "Noe", "Oliver", "Camille", "Farrell", "Ty", "Xiong", "Amayah", "Ramirez", "David", "Callahan", "Kimber", "Lane", "Matias", "Howard", "Sophie", "Garza", "Judah", "Benson", "Collins", "Tran", "Braxton", "Espinosa", "Braylee", "Guerra", "Leland", "McCormick", "Macie", "Preston", "Vincenzo", "Evans", "Eliana", "Walsh", "Bodhi", "Hardin", "Vada", "Harvey", "Cayden", "Jacobson", "Royal", "Gilbert", "Tobias", "Griffin", "Charlie", "Dougherty", "Brett", "Brady", "Ryan", "Calderon", "Oakley", "Cochran", "Alma", "Chavez", "Ian", "Hunt", "Genevieve", "Klein", "Marco", "Brock", "Jada", "Marks", "Amos", "Rose", "Magnolia", "Lambert", "Mario", "Mayer", "Ainhoa", "Francis", "Harvey", "Gordon", "Taylor", "Bryant", "Jonah", "Sanchez", "Aria", "Rich", "Miller", "Carlson", "Kali", "Randolph", "Eugene", "Villalobos", "Zoya", "Dominguez", "Kaden", "Larsen", "Xiomara", "Garcia", "James", "Beltran", "Kaydence", "Taylor", "Jackson", "Shaw", "Emersyn", "Avila", "Jaylen", "Garza", "River", "Bentley", "Randy", "McGee", "Kayleigh", "Brown", "Elijah", "McGuire"};

        return names[Random.Range(0, names.Length)];
    }
}