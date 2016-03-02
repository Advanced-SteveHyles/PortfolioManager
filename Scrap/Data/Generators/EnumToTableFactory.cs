namespace Data.Generators
{
    class EnumToTableFactory
    {
        //internal static void CreateAccountTypeTable(PortfolioManagerContext ctx, EnumAccountType accountType)
        //{            
        //    var  temp = new Ref.AccountType();

        //    switch (accountType)
        //    {
        //        case EnumAccountType.Test:
        //            temp = new Ref.AccountType(){AccountTypeID= (int)accountType , TypeName="Test"};
        //            break;
        //        default :
        //            temp = new Ref.AccountType(){AccountTypeID= (int)accountType , TypeName="Undefined"};
        //            break;

        //    }

        //    //See if the type exists
        //    var at = ctx.AccountTypes.Where(a=>a.AccountTypeID ==  (int)accountType).SingleOrDefault();

        //    if (at != null)                        
        //    {
        //        //Yes, patch it
        //        at.TypeName = temp.TypeName;
        //    }
        //    else
        //    {
        //        //No, Insert it
        //        ctx.AccountTypes.Add(temp);
        //    }
            
        //    ctx.SaveChanges();    
        //}

        //internal static void CreateTransactionTypeTable(PortfolioManagerContext ctx, EnumTransactionType transactionType)
        //{
        //    var temp = new Ref.TransactionType((int)transactionType, transactionType.ToString());

        //    switch (transactionType)
        //    {
        //        case EnumTransactionType.Test:
        //            temp = new Ref.TransactionType((int)transactionType, transactionType.ToString());
        //            break;
        //        default:
        //            temp = new Ref.TransactionType ((int)transactionType, "Undefined" );
        //            break;

        //    }

        //    //See if the type exists
        //    var at = ctx.TransactionTypes.Where(a => a.TransactionTypeID == (int)transactionType).SingleOrDefault();

        //    if (at != null)
        //    {
        //        //Yes, patch it
        //        at.TypeName = temp.TypeName;
        //    }
        //    else
        //    {
        //        //No, Insert it
        //        ctx.TransactionTypes.Add(temp);
        //    }

        //    ctx.SaveChanges();
        //}
    }
}
