import React, { Component } from 'react';
import _ from "lodash"

export class Home extends Component {
  static displayName = Home.name;

    constructor(props) {
        super(props);
        this.state = { postsUpVotes: [], userPosts: [], loading: true };
        this.loadingTimer = 0
    }

    componentDidMount() {
        this.populateHomeDataDummy().then(r => console.log("stats updated"));
        // this.loadingTimer = setIntertval(() => {
        //     this.populateHomeData()
        // }, 1000)
    }

    async populateHomeDataDummy() {
        const data =  {
            postsUpVotes: [{id: "123", title: "Post 1", count: 100}, {id: "124", title: "Post 2", count: 200}, {id: "124", title: "Post 2", count: 300}],
            userPosts: [{id: "847", name: "user1", count: 200}, {id: "847", name: "user 2", count: 300}]
        }
        this.setState({ postsUpVotes: data.postsUpVotes, userPosts : data.userPosts, loading: false });
    }

    async populateHomeData() {
        try {
            const response = await fetch('homeStats');
            const data = await response.json();
            this.setState({ postsUpVotes: data.postsUpVotes,userPosts : data.userPosts, loading: false });
        } catch (e) {
            console.log(e)
        }
    }
    
    static renderPostUpVotesTable(postsUpVotes) {
        if (!_.isArray(postsUpVotes)) return (<div></div>)

        return (<table className="table table-striped" aria-labelledby="tableLabel">
            <thead>
            <tr>
                <th>Title</th>
                <th>Up Votes Count</th>
            </tr>
            </thead>
            <tbody>
            {postsUpVotes.map(p =>
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

        const postUpVotesTable = Home.renderPostUpVotesTable(this.state.postsUpVotes)
        const userPostsTable = Home.renderUserPostsCount(this.state.userPosts)

        return (
            <div>
                <h1>Reddit Statistics</h1>

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
