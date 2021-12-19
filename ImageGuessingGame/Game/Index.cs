using System;
namespace ImageGuessingGame.GameContext
{
    public class Index
    {
        public Index(){}
        public Index(int indx){
            index=indx;
        }
        public int IndexId{get;set;}
        public int index{get;set;}

        public Guid OracleId{get;set;}
        public Oracle Oracle{get;set;}

    }
}