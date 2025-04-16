async function GetAll() {
    try{
        const response = await fetch('https:Api/stock/', {
            method : 'GET',
            headers: {
                'Content-Type': 'application/json'
            }
        });
    }
}