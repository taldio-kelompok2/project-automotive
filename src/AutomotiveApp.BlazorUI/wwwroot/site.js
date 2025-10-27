function scrollToTop() {
    window.scrollTo({ top: 0, behavior: 'smooth' });
}

window.loginViaFetch = async (url, body) => {
    const res = await fetch(url, {
        method: "POST",
        credentials: "include",
        headers: { "Content-Type": "application/json" },
        body
    });
    return res.ok;
}

window.logoutViaFetch = async (url, body) => {
    const res = await fetch(url, {
        method: "POST",
        credentials: "include",
        headers: { "Content-Type": "application/json" },
        body
    });
    return res.ok;
}

window.refreshViaFetch = async (url, body) => {
    const res = await fetch(url, {
        method: "POST",
        credentials: "include",
        headers: { "Content-Type": "application/json" },
        body
    });

    await new Promise(r => setTimeout(r, 150));
    
    return await res.text();
}

