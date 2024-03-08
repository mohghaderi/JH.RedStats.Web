import React, { Component } from 'react';
import _ from "lodash"

export class Home extends Component {
  static displayName = Home.name;

    constructor(props) {
        super(props);
        this.state = { postUpVotes: [], userPosts: [], loading: true, lastUpdated: "" };
        this.loadingTimer = 0
    }

    componentDidMount() {
        this.loadingTimer = setInterval(() => {
            this.populateHomeData().then(r => console.log("stats updated"));
        }, 1000)
    }
    
    componentWillUnmount() {
        clearInterval(this.loadingTimer)
    }
    
    // Dummy data for testing purposes only
    // async populateHomeDataDummy() {
    //     const data =  {
    //         postUpVotes: [{id: "123", title: "Post 1", count: 100}, {id: "124", title: "Post 2", count: 200}, {id: "125", title: "Post 2", count: 300}],
    //         userPosts: [{id: "user1", name: "User Name 1", count: 200}, {id: "user2", name: "User Name 2", count: 300}]
    //     }
    //     this.setState({ postUpVotes: data.postUpVotes, userPosts : data.userPosts, loading: false, lastUpdated: new Date() });
    // }

    async populateHomeData() {
        try {
            const requestOptions = {
                method: 'GET',
                headers: { 'Content-Type': 'application/json' },
                // body: JSON.stringify({ subReddit: "funny" }),
            };
            const query = + new URLSearchParams({
                subReddit: "funny",
            }).toString()
            const response = await fetch('/api/HomeStats?' + query, requestOptions);
            const data = await response.json();
            this.setState({ postUpVotes: data.postUpVotes,userPosts : data.userPosts, loading: false, lastUpdated: new Date() });
        } catch (e) {
            console.log(e)
        }
    }
    
    static renderPostUpVotesTable(postUpVotes) {
        if (!_.isArray(postUpVotes)) return (<div></div>)

        return (<table className="table table-striped" aria-labelledby="tableLabel">
            <thead>
            <tr>
                <th>Title</th>
                <th>Up Votes Count</th>
            </tr>
            </thead>
            <tbody>
            {postUpVotes.map(p =>
                <tr key={p.id}>
                    <td>{p.title}</td>
                    <td>{p.count}</td>
                </tr>
            )}
            </tbody>
        </table>)
    }
    
    static renderUserPostsCount(userPosts) {
        if (!_.isArray(userPosts)) return (<div>No data found!</div>)

        return (<table className="table table-striped" aria-labelledby="tableLabel">
            <thead>
            <tr>
                <th>User Name</th>
                <th>Posts Count</th>
            </tr>
            </thead>
            <tbody>
            {userPosts.map(p =>
                <tr key={p.id}>
                    <td>{p.name}</td>
                    <td>{p.count}</td>
                </tr>
            )}
            </tbody>
        </table>)
    }

    render() {
        if (this.state.loading) return <p><em>Loading...</em></p>

        const postUpVotesTable = Home.renderPostUpVotesTable(this.state.postUpVotes)
        const userPostsTable = Home.renderUserPostsCount(this.state.userPosts)

        return (
            <div>
                <h1>Reddit Statistics</h1>

                <div>lastUpdated: {this.state.lastUpdated.toISOString()}</div>
                <hr/>

                <div className={"row"}>
                <div className={"col col-6"}>
                    <h2>Posts with most up votes</h2>
                    {postUpVotesTable}
                </div>
                <div className={"col col-6"}>
                    <h2>Users with most posts</h2>
                    {userPostsTable}
                </div>
                </div>
            </div>
        );
    }
}
