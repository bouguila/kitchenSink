using Starcounter;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace KitchenSink
{
    [Database]
    public class Person
    {
        public string FirstName;
        public string LastName;
        public int Rank;
    }


    partial class SortableList : Json
    {
        protected override void OnData()
        {
            base.OnData();
            Debug.WriteLine("***************************************************************************");
            Db.Transact(() =>
            {
                var anyone = Db.SQL<Person>("SELECT p FROM Person p").First;
                if (anyone == null)
                {
                    new Person
                    {
                        FirstName = "John",
                        LastName = "Doe",
                        Rank = 1
                    };
                    new Person
                    {
                        FirstName = "test",
                        LastName = "user",
                        Rank = 2
                    };
                    new Person
                    {
                        FirstName = "jawher",
                        LastName = "bouguila",
                        Rank = 3
                    };
                    new Person
                    {
                        FirstName = "star",
                        LastName = "counter",
                        Rank = 4
                    };
                }
            });

            var person = Db.SQL<Person>("SELECT p FROM Person p");
            Persons.Data = person;
        }

        



    }
}
