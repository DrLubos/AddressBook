using AddressBook.CommonLibrary;
// Cela trieda generovana AI
public class SearchEventArgs : EventArgs
{
    public SearchResult FilteredResult { get; }

    public SearchEventArgs(SearchResult filteredResult)
    {
        FilteredResult = filteredResult;
    }
}