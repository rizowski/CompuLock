class AccountProgram < ActiveRecord::Base
  	attr_accessible :lastrun, :name, :open_count
  
  	validates :name, :presence => true

	belongs_to :account
end
