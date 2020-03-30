public interface IDataContextHandler<in T> where T : class
{
    T DataContext { set; }    
}