require "spec_helper"

describe Api::V1::TokensController do
	describe "Routing" do
		it "POST routes to #create" do
			{post: "api/v1/tokens"}.should route_to(
				controller: "api/v1/tokens", 
				action: "create", 
				format: "json")
		end

		it "GET doesnt route to #create" do
			{get: "api/v1/tokens"}.should_not route_to(
				controller: "api/v1/tokens", 
				action: "create", 
				format: "json")
		end

		it "DELETE routes to #destroy" do
			{delete: "api/v1/tokens/1"}.should route_to(
				controller: "api/v1/tokens", 
				action: "destroy",
				id: "1",
				format: "json")
		end
	end
end