import type Source from "@/model/Source"
import type SourcesFilterOptions from "@/model/SourcesFilterOptions"

export function filterSources(sources: Source[], sourcesFilterOptions: SourcesFilterOptions | undefined): Source[]{
    const searchValue = getFilterSearchValue(sourcesFilterOptions);

    if(searchValue === "") return sources;

    return sources.filter((source) => {
        return Object.values(source).some(val => {
            // if val is a string, check if it contains the search value
            if (isString(val)) { 
                return doesContainValue(val, searchValue);
            }

            //if val is an array, check if some of its elements contains the search value
            if (Array.isArray(val)) {  
                if((val.length > 0) && isString(val[0])){
                    return val.some(tag => tag.toLowerCase().includes(searchValue));
                }
            }

            //else is object
            return false;
        });
    });
}

function getFilterSearchValue(filters: SourcesFilterOptions | undefined): string{
    if(!filters) return "";
    
    const searchValue = filters.searchTextValue || "";

    return searchValue;
}

function isString(object: any): boolean{
    return (typeof object === 'string')
};

function doesContainValue(objectString: string, searchValue: string): boolean{
    return objectString.toLowerCase().includes(searchValue);
}