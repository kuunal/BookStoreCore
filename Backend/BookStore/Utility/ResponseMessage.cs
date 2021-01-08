using System;
using System.Collections.Generic;
using System.Text;

namespace ModelLayer
{
    /// <summary>
    /// Custom response messages.
    /// </summary>
    public class ResponseMessage
    {
        public const string LOGIN_SUCCESS = "Login successful";
        public const string LOGIN_FAILED = "Login failed! please enter proper id or password.";
        public const string USER_ADDED = "User created successfully";
        public const string BOOK_ADDED = "Book added successfully";
        public const string BOOK_REMOVE = "Book removed successfully";
        public const string BOOK_REMOVE_FAILED = "No such book exists!";
        public const string SUCCESSFUL = "Successful";
        public const string BOOK_NOT_FOUND = "No such book exists!";
        public const string BOOK_UPDATED = "Book updated successfully!";
        public const string CART_ITEM_NOT_FOUND = "No such item in cart!";

    }
}
