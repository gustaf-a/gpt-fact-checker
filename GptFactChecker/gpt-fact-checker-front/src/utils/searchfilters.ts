import type Source from "@/model/Source"
import type User from "@/model/User"
import type FilterOptions from "@/model/FilterOptions"

export function filterSources(sources: Source[], filterOptions: FilterOptions | undefined): Source[]{
    const searchValue = getFilterSearchValue(filterOptions);

    if(searchValue === "") return sources;

    return sources.filter((source) => {
        return Object.values(source).some(val => {
            // if val is a string, check if it contains the search value
            if (typeof val === 'string') { 
                return doesContainValue(val, searchValue);
            }

            //if val is an array, check if some of its elements contains the search value
            if (Array.isArray(val)) {  
                if((val.length > 0) && (typeof val[0] === 'string')){
                    return val.some(tag => tag.toLowerCase().includes(searchValue));
                }
            }

            //else is object
            return false;
        });
    });
}

export function filterUsers(users: User[], filterOptions: FilterOptions | undefined): User[]{
    const searchValue = getFilterSearchValue(filterOptions);

    if(searchValue === "") return users;

    return users.filter((user) => {
        return Object.values(user).some(val => {
            // if val is a string, check if it contains the search value
            if (typeof val === 'string') { 
                return doesContainValue(val, searchValue);
            }

            //if val is an array, check if some of its elements contains the search value
            if (Array.isArray(val)) {  
                if((val.length > 0) && (typeof val[0] === 'string')){
                    return val.some(tag => tag.toLowerCase().includes(searchValue));
                }
            }

            //else is object
            return false;
        });
    });
}

function getFilterSearchValue(filters: FilterOptions | undefined): string{
    if(!filters) return "";
    
    const searchValue = filters.searchTextValue || "";

    return searchValue;
}

function doesContainValue(objectString: string, searchValue: string): boolean{
    return objectString.toLowerCase().includes(searchValue);
}