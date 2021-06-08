using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    //Number of books for level
    public float NumberOfBooksToSpawn;
    //List of books to return before level is complete
    public List<GameObject> BooksToReturn;
    //Book Prefab
    public GameObject BookPrefab;
    //Selectable genres for book
    public string[] GenresToChoose;
    //Parent for storing all books
    public GameObject BookStorageOBJ;

    // Start is called before the first frame update
    private void Start()
    {
        //Spawn correct number of books for level
        for(int i = 0; i < NumberOfBooksToSpawn; i++)
        {
            //Spawn Book Object
            GameObject NewBook = Instantiate(BookPrefab, BookStorageOBJ.transform);
            //Add data component
            BookBase NewBookData = NewBook.AddComponent<BookBase>();
            //Set Data details
            NewBookData.Genre = GenresToChoose[Random.Range(0, GenresToChoose.Length)];
            NewBookData.BookNumber = i;
            //Add to stored books
            BooksToReturn.Add(NewBook);
        }

        GameObject Player = GameObject.FindGameObjectWithTag("Player");
        Player.GetComponent<PlayerInventory>().UpdateBookRemainingText();
    }

}
