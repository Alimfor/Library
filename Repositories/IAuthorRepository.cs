﻿using library.Models;
using Library.Models;
using Library.Utils;

namespace Library.Repositories;

public interface IAuthorRepository
{
    List<Author> GetAllAuthors();
    Author GetAuthorById(int id);
    int GetAuthorIdByFirstAndLastName(string firstName, string lastName);
    Result SaveAuthor(Author author);
    Result UpdateAuthor(Author author);
    Result DeleteAuthorById(int id);
    List<ListSelect> AuthorSelect();
}